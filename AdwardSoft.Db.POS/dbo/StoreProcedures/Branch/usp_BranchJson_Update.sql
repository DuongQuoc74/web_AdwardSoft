CREATE PROCEDURE [dbo].[usp_BranchJson_Update]
	@Data VARCHAR(MAX)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return AS BIT = 1 

	BEGIN TRY 
		BEGIN TRAN;
			-- Read Json String
			WITH q AS
			(
				SELECT [key] nodePath, 
					   CAST(JSON_VALUE(d.[value],'$.id') AS INT) [Id],
					   CAST(NULL AS INT) [ParentId],
					   CAST(JSON_QUERY(d.[value],'$.children') AS NVARCHAR(MAX)) [Children]
				FROM OPENJSON(@Data) d
				UNION ALL
				SELECT q.nodePath + '.' + d.[key] nodePath, 
					   CAST(JSON_VALUE(d.[value],'$.id') AS INT) [Id],
					   q.id [ParentId], 
					   CAST(JSON_QUERY(d.[value],'$.children') AS NVARCHAR(MAX)) [Children]
				FROM q
				OUTER APPLY OPENJSON(q.children) d
				WHERE q.children IS NOT NULL
			)
			SELECT Id, ROW_NUMBER() OVER (ORDER BY nodePath) [Sort], 
			(CASE 
				WHEN [ParentId] IS NULL THEN [Id]
				ELSE [ParentId] END) AS [ParentId]
			INTO #JsonData
			FROM q
			ORDER BY [Id]

			--- Update target table
			UPDATE pc
			SET pc.[Sort] = s.[Sort], pc.[ParentId] = s.[ParentId]
			FROM [dbo].[Branch] pc 
			INNER JOIN #JsonData s ON pc.[Id] = s.[Id]
			DROP TABLE IF EXISTS #JsonData;
		COMMIT
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN;
		THROW;
	END CATCH

	RETURN @return;
END


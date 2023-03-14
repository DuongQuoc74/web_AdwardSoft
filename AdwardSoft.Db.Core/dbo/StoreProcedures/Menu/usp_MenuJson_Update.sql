CREATE PROCEDURE [dbo].[usp_MenuJson_Update]
	@Data VARCHAR(MAX)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return AS BIT = 1 

	BEGIN TRY 
		BEGIN TRAN;
			-- Read Json String
			WITH Q AS
			(
				SELECT [key] nodePath, 
					   CAST(JSON_VALUE(d.[value],'$.id') AS INT) [Id],
					   CAST(NULL AS INT) [ParentId],
					   CAST(JSON_QUERY(d.[value],'$.children') AS NVARCHAR(MAX)) [Children]
				FROM OPENJSON(@Data) AS D
				UNION ALL
				SELECT Q.nodePath + '.' + D.[key] nodePath, 
					   CAST(JSON_VALUE(D.[value],'$.id') AS INT) [Id],
					   Q.id [ParentId], 
					   CAST(JSON_QUERY(D.[value],'$.children') AS NVARCHAR(MAX)) [Children]
				FROM Q
				OUTER APPLY OPENJSON(Q.children) D
				WHERE Q.children IS NOT NULL
			)

			SELECT Id, ROW_NUMBER() OVER (ORDER BY nodePath) [Order], 
			(CASE 
				WHEN [ParentId] IS NULL THEN [Id]
				ELSE [ParentId] END) AS [ParentId]
			INTO #JsonData
			FROM Q
			ORDER BY [Id]

			--- Update target table
			UPDATE M
			SET M.[Order] = S.[Order], M.[ParentId] = S.[ParentId]
			FROM [dbo].[Menu] AS M 
			INNER JOIN #JsonData AS S ON M.[Id] = S.[Id]

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

-- =======================================================  
-- Author:      <AdwardSoft>  
-- Create date: <Create Date: Nov 15, 2018>  
-- Description: <Description: Sorting Module by JsonData>  
-- =======================================================
CREATE PROCEDURE [dbo].[usp_ModuleSort_Update]
	@Data VARCHAR(MAX)
AS BEGIN
	SET NOCOUNT ON;
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
					   q.Id [ParentId], 
					   CAST(JSON_QUERY(d.[value],'$.children') AS NVARCHAR(MAX)) [Children]
				FROM q
				OUTER APPLY OPENJSON(q.children) d
				WHERE q.children IS NOT NULL
			)
			SELECT Id, ISNULL((select value + 1 from string_split([nodePath], '.') ORDER BY [nodePath] OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY), 0) as 'SortPartly',
			(CASE 
				WHEN [ParentId] IS NULL THEN [Id]
				ELSE [ParentId] END) AS [ParentId]
			INTO #JsonData
			FROM q

			--- Update target table
			UPDATE t
			SET t.[Sort] = s.[Sort], t.[ParentId] = s.[ParentId]
			FROM [dbo].[Module] t INNER JOIN (SELECT *, ROW_NUMBER() OVER (ORDER BY ParentId, [SortPartly]) [Sort] FROM  #JsonData ) s
			ON t.[Id] = s.[Id]

			DROP TABLE IF EXISTS #JsonData;
		COMMIT

		SELECT DISTINCT m.*, 
						(
							SELECT ',' + CAST(mt1.[Type] AS char(1)) AS [text()]
							FROM [dbo].[ModuleType] mt1
							WHERE mt1.ModuleId = mt2.ModuleId
							FOR XML PATH('')
						) AS 'Types'
		FROM [dbo].[ModuleType] mt2
		RIGHT JOIN [dbo].[Module] m ON m.Id = mt2.ModuleId
		ORDER BY m.[Sort] DESC
	END TRY
	BEGIN CATCH
		SELECT 0;
		ROLLBACK;
		THROW;
	END CATCH
END

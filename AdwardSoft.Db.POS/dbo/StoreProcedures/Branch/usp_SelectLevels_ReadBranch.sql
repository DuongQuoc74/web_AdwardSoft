CREATE PROCEDURE [dbo].[usp_SelectLevels_ReadBranch]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY

		;WITH [OrderedTree] ([Title], [Id], [ParentId], [Depth], [Location])
		AS (SELECT PD.[Name] AS [Title],
				   PD.[Id],
				   PD.[ParentId],
				   0 AS [Depth],
				   CAST(FORMAT(PD.[Sort], 'd3') AS NVARCHAR(MAX)) AS [Location]
			FROM [dbo].[Branch] AS PD
			WHERE  PD.[Id] = PD.[ParentId]
			UNION ALL
			SELECT PD.[Name] AS [Title], 
				   PD.[Id],
				   PD.[ParentId],
				   parent.[Depth] + 1 AS [Depth],
				   CAST(CONCAT(parent.[Location], '.', FORMAT(PD.[Sort], 'd3')) AS NVARCHAR(MAX)) AS [Location]
			FROM [Branch] PD
			INNER JOIN [OrderedTree] parent ON PD.[ParentId] = parent.[Id] AND PD.[ParentId] != PD.[Id])

		SELECT [Title] AS [Text], [Id] AS [Id], [ParentId], [Location], [Depth] AS [Level]
		FROM [OrderedTree]
		ORDER BY [Location]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

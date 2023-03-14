CREATE PROCEDURE [dbo].[usp_Select_ReadProduct]
AS
	SELECT P.[Id] AS [Id], p.[Name] AS [Text]
	FROM [dbo].[Product] AS P
	ORDER BY P.[Name] ASC
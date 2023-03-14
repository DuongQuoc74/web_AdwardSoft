CREATE PROCEDURE [dbo].[usp_Select_ReadCategory]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Category]
	WHERE [Status] = 1
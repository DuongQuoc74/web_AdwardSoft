CREATE PROCEDURE [dbo].[usp_Select_ReadUnit]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Unit]
	WHERE [Status] = 1
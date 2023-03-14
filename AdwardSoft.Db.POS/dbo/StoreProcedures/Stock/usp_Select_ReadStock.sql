CREATE PROCEDURE [dbo].[usp_Select_ReadStock]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Stock]
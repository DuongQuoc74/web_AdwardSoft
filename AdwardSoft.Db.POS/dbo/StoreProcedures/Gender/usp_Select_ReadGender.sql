CREATE PROCEDURE [dbo].[usp_Select_ReadGender]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Gender]

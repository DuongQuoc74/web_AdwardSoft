CREATE PROCEDURE [dbo].[usp_Select_ReadPosition]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Position]

CREATE PROCEDURE [dbo].[usp_Category_ReadSelect]
AS
	SELECT S.[Id], S.[Name]
	FROM [dbo].[Category] AS S	   
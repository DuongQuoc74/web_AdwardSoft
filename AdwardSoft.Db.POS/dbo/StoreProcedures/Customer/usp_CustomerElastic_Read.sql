CREATE PROCEDURE [dbo].[usp_CustomerElastic_Read]
AS
	SELECT [Id], [Name], [Phone]
	FROM [dbo].[Customer]

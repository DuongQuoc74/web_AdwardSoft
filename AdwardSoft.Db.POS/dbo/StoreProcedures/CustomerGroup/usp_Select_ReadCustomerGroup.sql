CREATE PROCEDURE [dbo].[usp_Select_ReadCustomerGroup]
(@Status TINYINT = 1)
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[CustomerGroup]
	WHERE [Status] = @Status
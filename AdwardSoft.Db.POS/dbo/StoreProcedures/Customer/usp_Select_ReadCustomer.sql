CREATE PROCEDURE [dbo].[usp_Select_ReadCustomer]
AS
SELECT [Id], [Name] as [Text]
FROM [dbo].[Customer]
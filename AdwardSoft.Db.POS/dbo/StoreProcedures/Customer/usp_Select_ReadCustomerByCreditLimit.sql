CREATE PROCEDURE [dbo].[usp_Select_ReadCustomerByCreditLimit]
AS
SELECT [Id], [Name] as [Text]
FROM [dbo].[Customer]
WHERE [CreditLimit] > 0
CREATE PROCEDURE [dbo].[usp_Select_ReadPaymentMethod]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[PaymentMethod]

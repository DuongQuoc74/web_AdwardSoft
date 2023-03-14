CREATE PROCEDURE [dbo].[usp_PaymentMethodRedis_Read]
AS
	SELECT P.*
	FROM [dbo].[PaymentMethod] AS P

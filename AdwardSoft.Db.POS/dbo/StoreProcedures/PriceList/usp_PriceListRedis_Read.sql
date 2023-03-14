CREATE PROCEDURE [dbo].[usp_PriceListRedis_Read]
AS
	SELECT P.*
	FROM [dbo].[PriceList] AS P
CREATE PROCEDURE [dbo].[usp_ProductUnitConverterRedis_Read]
AS
	SELECT P.*
	FROM [dbo].[ProductUnitConverter] AS P

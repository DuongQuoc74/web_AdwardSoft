CREATE PROCEDURE [dbo].[usp_StockRedis_Read]
AS
	SELECT S.*
	FROM [dbo].[Stock] AS S

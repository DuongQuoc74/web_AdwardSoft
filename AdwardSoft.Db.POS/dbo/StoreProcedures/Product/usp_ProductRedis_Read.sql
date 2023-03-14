CREATE PROCEDURE [dbo].[usp_ProductRedis_Read]
AS
SELECT P.*
FROM [dbo].[Product] AS P
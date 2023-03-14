CREATE PROCEDURE [dbo].[usp_SupplierRedis_Read]
AS
	SELECT S.*
	FROM [dbo].[Supplier] AS S

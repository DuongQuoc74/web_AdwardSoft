CREATE PROCEDURE [dbo].[usp_DeliveryPointRedis_Read]
AS
	SELECT L.*
	FROM [dbo].[DeliveryPoint] AS L

CREATE PROCEDURE [dbo].[usp_ServiceDisplayRedis_Read]
AS
	SELECT G.*
	FROM [dbo].[ServiceDisplay] AS G
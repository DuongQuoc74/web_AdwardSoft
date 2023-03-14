CREATE PROCEDURE [dbo].[usp_GenderRedis_Read]
AS
	SELECT G.*
	FROM [dbo].[Gender] AS G


CREATE PROCEDURE [dbo].[usp_UnitRedis_Read]
AS
	SELECT U.*
	FROM [dbo].[Unit] AS U

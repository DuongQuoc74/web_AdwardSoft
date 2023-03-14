CREATE PROCEDURE [dbo].[usp_PositionRedis_Read]
AS
	SELECT P.*
	FROM [dbo].[Position] AS P
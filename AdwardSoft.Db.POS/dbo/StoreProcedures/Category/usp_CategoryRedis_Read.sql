CREATE PROCEDURE [dbo].[usp_CategoryRedis_Read]
AS
	SELECT C.*
	FROM [dbo].[Category] AS C

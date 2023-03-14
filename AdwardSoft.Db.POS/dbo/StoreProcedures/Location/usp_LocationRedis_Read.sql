CREATE PROCEDURE [dbo].[usp_LocationRedis_Read]
AS
	SELECT L.*
	FROM [dbo].[Location] AS L
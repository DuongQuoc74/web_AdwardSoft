CREATE PROCEDURE [dbo].[usp_ScoreConfigurationRedis_Read]
AS
SELECT S.*
FROM [dbo].[ScoreConfiguration] AS S

CREATE PROCEDURE [dbo].[usp_CustomerRedis_ReadDefaulting]
AS
	SELECT TOP 1 * 
	FROM [dbo].[Customer]
	WHERE [IsDefault] = 1
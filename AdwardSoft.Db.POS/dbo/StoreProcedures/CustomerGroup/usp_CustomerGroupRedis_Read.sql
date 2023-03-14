CREATE PROCEDURE [dbo].[usp_CustomerGroupRedis_Read]
AS
	SELECT C.*
	FROM [dbo].[CustomerGroup] AS C

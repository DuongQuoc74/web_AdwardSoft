CREATE PROCEDURE [dbo].[usp_MembershipClassRedis_ReadDefault]
AS
	SELECT TOP 1 *
	FROM [dbo].[MembershipClass] 
	WHERE [IsDefault] = 1

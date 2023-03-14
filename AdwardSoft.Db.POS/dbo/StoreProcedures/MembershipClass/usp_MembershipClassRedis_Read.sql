CREATE PROCEDURE [dbo].[usp_MembershipClassRedis_Read]
AS
	SELECT M.*
	FROM [dbo].[MembershipClass] AS M

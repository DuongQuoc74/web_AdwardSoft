CREATE PROCEDURE [dbo].[usp_MembershipClass_ReadDefault]
AS
	SELECT *
	FROM [dbo].[MembershipClass]
	WHERE [IsDefault] = 1

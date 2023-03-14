CREATE PROCEDURE [dbo].[usp_MembershipClass_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[MembershipClass] 
	WHERE [Id] = @Id

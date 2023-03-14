CREATE PROCEDURE [dbo].[usp_RoleConfig_ReadById]
	@Id INT
AS
BEGIN
	SELECT r.Id AS 'RoleId', rc.IsLogging, rc.VerificationMethod, rc.PwdRegex
	FROM RoleConfig rc
	RIGHT JOIN [Role] r ON rc.RoleId = r.Id
	WHERE r.Id = @Id
END
CREATE PROCEDURE [dbo].[usp_MailServer_ReadEmailIsExist]
	@Email VARCHAR(2048),
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT TOP 1 1 FROM [dbo].[MailServer] WHERE [Email] = @Email AND Id <> @Id)
 		SELECT 1
	ELSE
		SELECT 0
END
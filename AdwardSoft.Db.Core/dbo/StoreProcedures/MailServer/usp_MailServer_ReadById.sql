CREATE PROCEDURE [dbo].[usp_MailServer_ReadById]
	@Id INT
AS
	SELECT * FROM [MailServer] WHERE [Id] = @Id
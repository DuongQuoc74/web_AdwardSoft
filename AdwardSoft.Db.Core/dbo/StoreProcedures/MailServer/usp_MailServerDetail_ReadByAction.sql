CREATE PROCEDURE [dbo].[usp_MailServerDetail_ReadByAction]
	@Action INT,
	@Type TINYINT
AS
	SELECT MS.*, NT.[Action], NT.[Content], NT.[IsActive], NT.[Title], NT.[Type] FROM [MailServer] AS MS
	INNER JOIN [dbo].[NotificationTemplate] AS NT ON MS.[Id] = NT.[MailServerId]
	WHERE NT.[Action] = @Action AND NT.[Type] = @Type
CREATE PROCEDURE [dbo].[usp_NotificationTemplate_Create]
	@Id INT,
	@Action TINYINT, 
	@Type TINYINT,
	@Title NVARCHAR(150),
	@Content NVARCHAR(4000),
	@IsActive BIT,
	@MailServerId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS (SELECT TOP 1 1 FROM [NotificationTemplate] WHERE [Type] = @Type AND [Action] = @Action)
			BEGIN
				INSERT INTO [NotificationTemplate] ([Type], [Action], [IsActive], [Title], [Content], [MailServerId])
				VALUES (@Type, @Action, @IsActive, @Title, @Content, @MailServerId)
				SELECT 1
			END
			ELSE 
			BEGIN
				SELECT -1
			END
		COMMIT
	END TRY
	BEGIN CATCH
		SELECT 0
		ROLLBACK;
		THROW;
	END CATCH
END
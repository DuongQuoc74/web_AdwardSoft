CREATE PROCEDURE [dbo].[usp_NotificationTemplate_Update]
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
			UPDATE [NotificationTemplate]
			SET [Type] = @Type, [Action] = @Action, [IsActive] = @IsActive, [Title] = @Title, [Content] = @Content, [MailServerId] = @MailServerId
			WHERE [Id] = @Id
			SELECT 1
		COMMIT
	END TRY
	BEGIN CATCH
		SELECT 0
		ROLLBACK;
		THROW;
	END CATCH
END
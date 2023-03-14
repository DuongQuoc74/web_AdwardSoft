CREATE PROCEDURE [dbo].[usp_HandleError_Update]
	@Id INT,
	@StatusCode NUMERIC(3,0), 
    @LanguageCode CHAR(2), 
    @Title NVARCHAR(150),
	@Message NVARCHAR(MAX)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	DECLARE @NewId INT
	
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[HandleError]
			SET [StatusCode] = @StatusCode
			WHERE [Id] = @Id
			IF EXISTS (SELECT TOP 1 1 FROM [dbo].[HandleErrorTrans] WHERE [HandleErrorId] = @Id AND [LanguageCode] = @LanguageCode)
			BEGIN
				UPDATE [dbo].[HandleErrorTrans]
				SET [Title] = @Title,
				    [Message] = @Message
				WHERE [HandleErrorId] = @Id AND [LanguageCode] = @LanguageCode
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[HandleErrorTrans] ([HandleErrorId], [LanguageCode], [Title], [Message])
				VALUES (@Id, @LanguageCode, @Title, @Message)
			END
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

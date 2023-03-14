CREATE PROCEDURE [dbo].[usp_User_Delete]
	@Id BIGINT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	DECLARE @Return INT = 1;
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[User]
			WHERE [Id] = @Id

			DELETE FROM [dbo].[UserRole]
			WHERE [UserId] = @Id

			DELETE FROM [dbo].[UserCipher]
			WHERE [UserId] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @Return = 0;
		THROW;
	END CATCH
	SELECT @Return;
	RETURN @Return;
END

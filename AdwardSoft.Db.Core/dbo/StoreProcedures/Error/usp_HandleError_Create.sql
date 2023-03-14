CREATE PROCEDURE [dbo].[usp_HandleError_Create]
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
			INSERT INTO [dbo].[HandleError] ([StatusCode]) VALUES (@StatusCode)
			INSERT INTO [dbo].[HandleErrorTrans] ([HandleErrorId], [LanguageCode], [Title], [Message]) 
			VALUES (SCOPE_IDENTITY(), @LanguageCode, @Title, @Message )
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END


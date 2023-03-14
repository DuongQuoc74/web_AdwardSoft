CREATE PROCEDURE [dbo].[usp_MenuTranslation_Update]
	@MenuId INT,
	@LanguageCode CHAR(2),
	@NavigationLabel NVARCHAR(70),
	@URL NVARCHAR(2048)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[MenuTranslation] WHERE [MenuId] = @MenuId AND [LanguageCode] = @LanguageCode)
			BEGIN
				INSERT INTO [dbo].[MenuTranslation] ([MenuId], [LanguageCode], [NavigationLabel], [URL])
				VALUES(@MenuId, @LanguageCode, @NavigationLabel, @URL)	
			END
			ELSE
			BEGIN
				UPDATE [dbo].[MenuTranslation]
				SET [NavigationLabel] = @NavigationLabel, [URL] = @URL
				WHERE [MenuId] = @MenuId AND [LanguageCode] = @LanguageCode
			END
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

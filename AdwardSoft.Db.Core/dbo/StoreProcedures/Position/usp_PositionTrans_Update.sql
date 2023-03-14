CREATE PROCEDURE [dbo].[usp_PositionTrans_Update]
	@PositionId INT,
	@LanguageCode CHAR(2),
	@Title NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF EXISTS(SELECT TOP 1 1 FROM PositionTrans WHERE PositionId = @PositionId AND LanguageCode = @LanguageCode)
				UPDATE PositionTrans
				SET [Title] = @Title
				WHERE PositionId = @PositionId AND LanguageCode = @LanguageCode
			ELSE
				INSERT INTO PositionTrans ([PositionId], [LanguageCode], [Title])
				VALUES(@PositionId, @LanguageCode, @Title)
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		RETURN 0;
		THROW;
	END CATCH
END
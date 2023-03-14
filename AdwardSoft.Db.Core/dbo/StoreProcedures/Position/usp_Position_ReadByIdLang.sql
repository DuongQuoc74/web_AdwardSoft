CREATE PROCEDURE [dbo].[usp_Position_ReadByIdLang]
	@Id INT,
	@LanguageCode CHAR(2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			IF(@LanguageCode IS NULL)
				SELECT @LanguageCode = Code FROM [Language] WHERE IsDefault = 1

			SELECT * 
			FROM Position p
			LEFT JOIN PositionTrans pt ON pt.PositionId = p.Id
			WHERE p.Id = @Id AND pt.LanguageCode = @LanguageCode
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_Language_Delete]
	@Code CHAR(2)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			IF EXISTS (SELECT 1 FROM  [dbo].[Language] WHERE [Code] = @Code AND IsDefault = 1)
			BEGIN
				DECLARE @newDefault CHAR(2)
				SELECT TOP 1 Code=@newDefault FROM [dbo].[Language] WHERE [IsDefault] = 0
				UPDATE [dbo].[Language] SET [IsDefault] = 1 WHERE [Code] = @newDefault
			END
			BEGIN TRY
			DELETE FROM [dbo].[Language]
			WHERE [Code] = @Code
			END TRY
			BEGIN CATCH
				SET @return = 2;
				THROW;
			END CATCH
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0;
		THROW;
	END CATCH
	SELECT @return
END

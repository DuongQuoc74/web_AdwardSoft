CREATE PROCEDURE [dbo].[usp_Language_Create]
	@Code CHAR(2),
    @Name NVARCHAR(30), 
	@IsDefault INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			IF (@IsDefault = 1)
			BEGIN
				UPDATE [dbo].[Language]
				SET [IsDefault] = 0			
			END
			INSERT INTO [dbo].[Language] 
			VALUES(@Code, @Name, @IsDefault)
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

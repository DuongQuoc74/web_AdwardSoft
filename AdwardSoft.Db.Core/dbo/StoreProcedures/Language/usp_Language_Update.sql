CREATE PROCEDURE [dbo].[usp_Language_Update]
	@Code CHAR(2),
    @Name NVARCHAR(30), 
	@IsDefault INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
--	DECLARE @return BIT = 1
--	BEGIN TRY
--		BEGIN TRAN;
--			IF (@IsDefault = 1)
--			BEGIN
--				UPDATE [dbo].[Language]
--				SET [IsDefault] = 0
--				WHERE [IsDefault] = 1
--			END
--			UPDATE [dbo].[Language]
--			SET [IsDefault] = @IsDefault,
--				[Name] = @Name
--			WHERE [Code] = @Code
--		COMMIT	
--	END TRY
--	BEGIN CATCH
--		SET @return = 0
--		ROLLBACK TRAN
--		THROW;
--	END CATCH
--	RETURN @return;
--END
-----
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			IF (@IsDefault = 1)
			BEGIN
				UPDATE [dbo].[Language]
				SET [IsDefault] = 0
				WHERE [IsDefault] = 1
			END
			ELSE
				UPDATE [dbo].[Language]
				SET [IsDefault] = 1
				WHERE [dbo].[Language].Code=(SELECT TOP 1 Code from [dbo].[Language])
			UPDATE [dbo].[Language]
			SET [IsDefault] = @IsDefault,
				[Name] = @Name
			WHERE [Code] = @Code
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

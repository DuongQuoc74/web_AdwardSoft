CREATE PROCEDURE [dbo].[usp_Customer_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			-- Check is Default
			DECLARE @IsDefault BIT;

			SELECT @IsDefault = [IsDefault]
			FROM [dbo].[Customer]
			WHERE [Id] = @Id

			-- If default true 
			-- Then set new default
			IF(@IsDefault = 1)
			BEGIN
				DECLARE @newDefault INT = 0;

				SELECT TOP 1 @newDefault = [Id]
				FROM [dbo].[Customer]
				WHERE [IsDefault] = 0
				ORDER BY [Id] ASC

				UPDATE [dbo].[Customer]
				SET [IsDefault] = 1
				WHERE [Id] = @newDefault
			END

			DELETE [dbo].[Customer]
			WHERE [Id] = @Id

		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

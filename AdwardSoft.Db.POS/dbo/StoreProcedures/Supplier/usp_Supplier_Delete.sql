CREATE PROCEDURE [dbo].[usp_Supplier_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DECLARE @count INT = (SELECT COUNT(*) FROM [dbo].[Supplier] WHERE [Id] <> @Id)

			IF(@count != 0)
			BEGIN 
				DECLARE @IsDefault BIT = 0
				SELECT @IsDefault = [IsDefault] FROM [dbo].[Supplier] WHERE [Id] = @Id

				DELETE FROM [dbo].[Supplier]
				WHERE [Id] = @Id

				IF (@IsDefault = 1)
				BEGIN
					UPDATE [dbo].[Supplier]
					SET [IsDefault] = @IsDefault
					WHERE [Id] = (SELECT TOP 1 [Id] FROM [dbo].[Supplier])
				END
			END 
		COMMIT
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END
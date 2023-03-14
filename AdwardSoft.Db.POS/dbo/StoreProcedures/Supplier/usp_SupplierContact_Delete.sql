CREATE PROCEDURE [dbo].[usp_SupplierContact_Delete]
	@SupplierId INT, 
	@Idx INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DECLARE @Count INT = 0
			DECLARE @IsDefault BIT = 0

			SELECT @Count = COUNT([Idx]) FROM [dbo].[SupplierContact] WHERE [SupplierId] = @SupplierId
			SELECT @IsDefault = [IsDefault] FROM [dbo].[SupplierContact] WHERE [SupplierId] = @SupplierId AND [Idx] = @Idx

			IF (@Count > 1)
			BEGIN
				DELETE FROM [dbo].[SupplierContact]
				WHERE [SupplierId] = @SupplierId AND [Idx] = @Idx
				IF (@IsDefault = 1)
				BEGIN
					UPDATE [dbo].[SupplierContact]
					SET [IsDefault] = @IsDefault
					WHERE [SupplierId] = @SupplierId AND [Idx] = (SELECT TOP 1 [Idx] FROM [dbo].[SupplierContact] WHERE [SupplierId] = @SupplierId)
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

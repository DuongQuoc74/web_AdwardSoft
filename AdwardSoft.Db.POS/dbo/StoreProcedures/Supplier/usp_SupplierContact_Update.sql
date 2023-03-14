CREATE PROCEDURE [dbo].[usp_SupplierContact_Update]
	@SupplierId INT, 
	@Idx INT, 
    @Phone CHAR(10), 
    @Name NVARCHAR(32), 
    @Position NVARCHAR(50), 
    @Note NVARCHAR(100),
	@IsDefault BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
				IF (@IsDefault = 1)
				BEGIN
					UPDATE [dbo].[SupplierContact]
					SET [IsDefault] = 0
					WHERE [SupplierId] = @SupplierId
				END

				DECLARE @Count INT = 0

				SELECT @Count = COUNT([Idx]) FROM [dbo].[SupplierContact] WHERE [SupplierId] = @SupplierId

				IF (@Count = 0) SET @IsDefault = 1

				UPDATE [dbo].[SupplierContact]
				SET [IsDefault] = @IsDefault,
					[Phone] = @Phone,
					[Name] = @Name,
					[Position] = @Position,
					[Note] = @Note
				WHERE [SupplierId] = @SupplierId AND [Idx] = @Idx
		COMMIT
		SELECT 1 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

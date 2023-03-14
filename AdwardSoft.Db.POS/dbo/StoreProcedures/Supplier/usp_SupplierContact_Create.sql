CREATE PROCEDURE [dbo].[usp_SupplierContact_Create]
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

				SELECT @Idx = ISNULL((MAX([Idx])+1),1) FROM [dbo].[SupplierContact] WHERE [SupplierId] = @SupplierId
				INSERT INTO [dbo].[SupplierContact] ([SupplierId], [Idx], [Phone], [Name], [Position], [Note], [IsDefault])
				VALUES(@SupplierId, @Idx, @Phone, @Name, @Position, @Note, @IsDefault)
		COMMIT
		SELECT 1 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

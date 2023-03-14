CREATE PROCEDURE [dbo].[usp_SupplierContact_ReadById]
	@SupplierId INT, 
	@Idx INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * FROM [dbo].[SupplierContact]
		WHERE [SupplierId] = @SupplierId AND [Idx] = @Idx
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

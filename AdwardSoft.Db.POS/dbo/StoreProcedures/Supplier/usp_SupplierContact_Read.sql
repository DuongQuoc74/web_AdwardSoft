CREATE PROCEDURE [dbo].[usp_SupplierContact_Read]
	@SupplierId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * FROM [dbo].[SupplierContact]
		WHERE [SupplierId] = @SupplierId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
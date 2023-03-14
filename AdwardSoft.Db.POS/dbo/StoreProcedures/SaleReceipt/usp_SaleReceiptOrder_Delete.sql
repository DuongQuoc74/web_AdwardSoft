CREATE PROCEDURE [dbo].[usp_SaleReceiptOrder_Delete]
	@SaleReceiptId CHAR(32), 
	@CustomerOrderId CHAR(32)
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[SaleReceiptOrder]
			WHERE [SaleReceiptId] = @SaleReceiptId AND [CustomerOrderId] = @CustomerOrderId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

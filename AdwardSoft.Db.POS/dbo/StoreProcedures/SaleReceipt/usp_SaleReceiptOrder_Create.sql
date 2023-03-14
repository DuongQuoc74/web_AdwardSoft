CREATE PROCEDURE [dbo].[usp_SaleReceiptOrder_Create]
	@SaleReceiptId CHAR(32), 
	@CustomerOrderId CHAR(32)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[SaleReceiptOrder] (	[SaleReceiptId], 
                                                    [CustomerOrderId] )
			VALUES(	@SaleReceiptId, 
                    @CustomerOrderId )	                    		
		COMMIT	
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_SaleReceipt_Delete]
	@Id CHAR(32)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[SaleReceiptScore]
			WHERE [SaleReceiptId] = @Id

			DELETE FROM [dbo].[SaleReceiptOrder]
			WHERE [SaleReceiptId] = @Id

			DELETE FROM [dbo].[SaleReceiptDetail]
			WHERE [SaleReceiptId] = @Id

			DELETE FROM [dbo].[SaleReceiptPromotion]
			WHERE [SaleReceiptId] = @Id

			DELETE FROM [dbo].[SaleReceipt]
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

CREATE PROCEDURE [dbo].[usp_SaleReceiptDetail_Delete]
	@SaleReceiptId CHAR(32),
	@SaleReceiptDetailId CHAR(32),
	@IsAll BIT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			IF (@IsAll = 0)
			BEGIN
				DELETE FROM [dbo].[SaleReceiptDetail]
				WHERE [SaleReceiptDetailId] = @SaleReceiptDetailId

				DELETE FROM [dbo].[SaleReceiptPromotion]
				WHERE [SaleReceiptDetailId] = @SaleReceiptDetailId
			END
			ELSE
			BEGIN
				DELETE FROM [dbo].[SaleReceiptDetail]
				WHERE [SaleReceiptId] = @SaleReceiptId

				DELETE FROM [dbo].[SaleReceiptPromotion]
				WHERE [SaleReceiptId] = @SaleReceiptId
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

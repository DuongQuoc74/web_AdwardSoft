CREATE PROCEDURE [dbo].[usp_SaleReceiptDetail_Update]
	@SaleReceiptId CHAR(32), 
    @SaleReceiptDetailId CHAR(32), 
    @ProductId INT, 
    @Quantity NUMERIC(13, 3), 
    @UnitId INT, 
    @Price NUMERIC(16, 2), 
    @PromotionId INT, 
    @Discount NUMERIC(16, 2), 
    @Amount NUMERIC(16, 2), 
    @IsPromo BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
            UPDATE [dbo].[SaleReceiptDetail]
            SET    [Quantity] = @Quantity,
                   [UnitId] = @UnitId,
                   [Price] = @Price,
                   [Discount] = @Discount,
                   [Amount] = @Amount,
                   [IsPromo] = @IsPromo,
                   [ProductId] = @ProductId
            WHERE  [SaleReceiptDetailId] = @SaleReceiptDetailId	
            
            IF(@PromotionId > 0)
            BEGIN
                DELETE FROM [dbo].[SaleReceiptPromotion] WHERE [SaleReceiptDetailId] = @SaleReceiptDetailId AND [PromotionId] = @PromotionId AND [SaleReceiptId] = @SaleReceiptId
                INSERT INTO [dbo].[SaleReceiptPromotion] VALUES(@SaleReceiptDetailId, @PromotionId, @SaleReceiptId)
            END
		COMMIT	
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END


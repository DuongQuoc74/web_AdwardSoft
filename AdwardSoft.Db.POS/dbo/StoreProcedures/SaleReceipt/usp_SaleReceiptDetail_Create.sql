CREATE PROCEDURE [dbo].[usp_SaleReceiptDetail_Create]
	@SaleReceiptId CHAR(32), 
    @SaleReceiptDetailId CHAR(32), 
    @ProductId INT, 
    @Quantity NUMERIC(13, 3), 
    @UnitId INT, 
    @Price NUMERIC(16, 2), 
    @PromotionId INT, 
    @Discount NUMERIC(16, 2), 
    @Amount NUMERIC(16, 2), 
    @IsPromo BIT,
    @RetailPrice NUMERIC(16, 2),
    @CardPinCode VARCHAR(50) = '',
    @CardTelco VARCHAR(4) = '',
    @CardSerial VARCHAR(50) = '',
    @CardAmount NUMERIC(16, 2), 
    @CardTrace VARCHAR(20) = ''
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
            SET @SaleReceiptDetailId =  REPLACE(NEWID(), '-','')
			INSERT INTO [dbo].[SaleReceiptDetail] (	[SaleReceiptId], 
                                                    [SaleReceiptDetailId],
                                                    [ProductId], 
                                                    [Quantity], 
                                                    [UnitId], 
                                                    [Price], 
                                                    [Discount], 
                                                    [Amount], 
                                                    [IsPromo],
                                                    [RetailPrice],
                                                    [CardPinCode], [CardSerial],[CardTelco],
                                                    [CardAmount],[CardTrace]
                                                    )
			VALUES(	@SaleReceiptId, 
                    @SaleReceiptDetailId,
                    @ProductId, 
                    @Quantity, 
                    @UnitId, 
                    @Price, 
                    @Discount, 
                    @Amount, 
                    @IsPromo,
                    @RetailPrice, @CardPinCode, @CardSerial, @CardTelco, @CardAmount, @CardTrace)	   
             
             --Promotion
             IF(@PromotionId > 0)
                INSERT INTO [dbo].[SaleReceiptPromotion] VALUES (@SaleReceiptDetailId, @PromotionId, @SaleReceiptId)

		COMMIT	
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

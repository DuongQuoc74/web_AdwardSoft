CREATE PROCEDURE [dbo].[usp_SaleReceipt_Create]
	@Id CHAR(32) , 
    @Date SMALLDATETIME , 
    @CustomerId INT , 
    @BranchId INT , 
    @No CHAR(15) , -- Regex: “SR“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7), Format: example: SR 01 20 12 0000001 → SR0120120000001 
    @Description NVARCHAR(128) , 
    @Status TINYINT , -- 0 : Hoá đơn được lập, 1 : Hoá đơn bị huỷ
    @IsShipping BIT , 
    @IsCustomerOrder BIT  , 
    @TotalQuantity NUMERIC(13, 3) , 
    @SubTotal NUMERIC(16, 2) , 
    @ShippingFee NUMERIC(16, 2) , 
    @TaxRate NUMERIC(5, 2) , 
    @TaxFee NUMERIC(16, 2) , 
    @TotalDiscount NUMERIC(16, 2) , 
    @TotalAmount NUMERIC(16, 2) , 
    @PaymentMethodId INT , 
    @CreateDate SMALLDATETIME , 
    @CreatedUser BIGINT , 
    @ModifiedDate SMALLDATETIME , 
    @ModifiedUser BIGINT , 
    @EmployeeId BIGINT , 
    @ShiftId INT , 
    @CheckoutCounterId INT,
    @PriceType TINYINT = 0,
    @StockId INT = 0,
    @ExchangePoint NUMERIC(9,0) = 0,
    @ExchangeAmount NUMERIC(16,2) = 0,
    @Cash NUMERIC(16,2) = 0,
    @SaleReceiptIdRef VARCHAR(32) = ''
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
            DECLARE @Index INT 
            
            SET @Index = 1

            SET @Date = GETDATE()
            -- SAMPLE DATA - EDIT AFTER FINISH SHIFT/CHECKCOUNTER/EMPLOYEE

            --SELECT TOP 1 @BranchId = [Id] FROM [dbo].[Branch] 
            SELECT TOP 1 @CheckoutCounterId = [Id] FROM [dbo].[CheckoutCounter] 
            SELECT TOP 1 @ShiftId = [Id] FROM [dbo].[Shift] 
            SELECT TOP 1 @StockId = [Id] FROM [dbo].[Stock] WHERE [BranchId] = @BranchId AND [IsDefault] = 1
            --============================================================

			SELECT @Index = ISNULL(COUNT(s.[Id]), 0) + 1 
			FROM [dbo].[SaleReceipt] s			
			WHERE MONTH(s.CreateDate) = MONTH(GETDATE()) AND YEAR(s.CreateDate) = YEAR(GETDATE())

            SET @Id = REPLACE(NEWID(), '-','')
            SET @No = 'SR' + FORMAT(@CheckoutCounterId, 'd2') +   FORMAT(GETDATE(), 'yy') +  FORMAT(GETDATE(), 'MM') + FORMAT(@Index, 'd7')

			INSERT INTO [dbo].[SaleReceipt] ([Id],
                                             [Date],
                                             [CustomerId],
                                             [BranchId],
                                             [No],
                                             [Description],
                                             [Status],
                                             [IsShipping],
                                             [IsCustomerOrder],
                                             [TotalQuantity],
                                             [SubTotal],
                                             [ShippingFee],
                                             [TaxRate],
                                             [TaxFee],
                                             [TotalDiscount],
                                             [TotalAmount],
                                             [PaymentMethodId],
                                             [CreateDate],
                                             [CreatedUser],
                                             [ModifiedDate],
                                             [ModifiedUser],
                                             [EmployeeId] , 
                                             [ShiftId]  , 
                                             [CheckoutCounterId],
                                             [PriceType],[StockId],
                                             [ExchangePoint],[ExchangeAmount], [Cash],[SaleReceiptIdRef]
                                             )
			VALUES(	@Id, 
                    @Date, 
                    @CustomerId, 
                    @BranchId, 
                    @No, 
                    @Description, 
                    @Status, 
                    @IsShipping, 
                    @IsCustomerOrder, 
                    @TotalQuantity, 
                    @SubTotal, 
                    @ShippingFee, 
                    @TaxRate, 
                    @TaxFee, 
                    @TotalDiscount, 
                    @TotalAmount, 
                    @PaymentMethodId, 
                    GETDATE(), 
                    @CreatedUser, 
                    GETDATE(), 
                    @ModifiedUser,
                    @EmployeeId, 
                    @ShiftId, 
                    @CheckoutCounterId, 
                    @PriceType,@StockId,
                    @ExchangePoint, @ExchangeAmount, @Cash, @SaleReceiptIdRef)			
		COMMIT	
		SELECT @Id;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT '';
		THROW;
	END CATCH
END

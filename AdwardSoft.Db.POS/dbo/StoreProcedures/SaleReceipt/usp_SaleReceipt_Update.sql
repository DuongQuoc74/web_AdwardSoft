CREATE PROCEDURE [dbo].[usp_SaleReceipt_Update]
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
    @PriceType TINYINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
            UPDATE [dbo].[SaleReceipt]
            SET    [CustomerId] = @CustomerId,
                   [BranchId] = @BranchId,
				   [Description] = @Description,
                   [Status] = @Status,
                   [IsShipping] = @IsShipping,
                   [IsCustomerOrder] = @IsCustomerOrder,
                   [TotalQuantity] = @TotalQuantity,
                   [SubTotal] = @SubTotal,
                   [ShippingFee] = @ShippingFee,
                   [TaxRate] = @TaxRate,
                   [TaxFee] = @TaxFee,
                   [TotalDiscount] = @TotalDiscount,
                   [TotalAmount] = @TotalAmount,
                   [PaymentMethodId] = @PaymentMethodId,
                   [ModifiedDate] = GETDATE(),
                   [ModifiedUser] = @ModifiedUser, [PriceType] = @PriceType
            WHERE  [Id] = @Id          		
		COMMIT	
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

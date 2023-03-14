CREATE PROCEDURE [dbo].[usp_CustomerOrder_Create]
	@Id CHAR(32), 
    @Date DATE, 
    @CustomerId INT, 
    @BranchId INT, 
    @No CHAR(10), 
    @Description NVARCHAR(128), 
    @DeliveryDate SMALLDATETIME, 
    @DeliveryPointId INT, 
    @DeliveryVehicleId INT, 
    @Receiver NVARCHAR(120), 
    @ReceiverPhone VARCHAR(12), 
    @ReceiverAddress NVARCHAR(128), 
    @DeliveryType TINYINT,
    @VoucherCode VARCHAR(60), 
    @Status TINYINT = 0,
    @IsShipping BIT, 
    @TotalQuantityReg NUMERIC(8, 3), 
    @TotalQuantity NUMERIC(8, 3), 
    @SubTotal NUMERIC(16, 2), 
    @ShippingFee NUMERIC(16, 2), 
    @TaxRate NUMERIC(5, 2), 
    @TaxFee NUMERIC(16, 2), 
    @TotalDiscount NUMERIC(16, 2), 
    @TotalAmount NUMERIC(16, 2), 
    @PaymentMethodId INT, 
    @CreateDate SMALLDATETIME, 
    @CreatedUser BIGINT, 
    @ModifiedDate SMALLDATETIME, 
    @ModifiedUser BIGINT,
    @PaymentDate SMALLDATETIME,
    @PaymentUser BIGINT,
    @PaymentStatus TINYINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN	

                DECLARE @Code CHAR(2) = (SELECT 'SO')
                DECLARE @CurrentIdx INT = (SELECT [CurrentIdx] FROM [dbo].[ReceiptSetting] WHERE [Code] = @Code)
               

                SET @Id = REPLACE(NEWID(), '-','');

                SET @No = [dbo].[fn_ReceiptCalc](@Code)
                
                IF @CurrentIdx IS NULL
                    UPDATE [dbo].[ReceiptSetting] SET [CurrentIdx] = [StartNo]
                    WHERE [Code] = @Code
                ELSE
                    UPDATE [dbo].[ReceiptSetting] SET [CurrentIdx] = [CurrentIdx] + 1
                    WHERE [Code] = @Code

                SET @TotalQuantity = @TotalQuantityReg

                SET @TotalAmount = @SubTotal - @TotalDiscount + @ShippingFee + @TaxFee
				
                INSERT INTO [dbo].[CustomerOrder] 
                      ( [Id], 
                        [Date], 
                        [CustomerId],
                        [BranchId], 
                        [No], 
                        [Description], 
                        [DeliveryDate], 
                        [DeliveryPointId], 
                        [DeliveryVehicleId], 
                        [Receiver], 
                        [ReceiverPhone], 
                        [ReceiverAddress], 
                        [DeliveryType],
                        [VoucherCode], 
                        [Status],
                        [IsShipping], 
                        [TotalQuantityReg], 
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
                        [PaymentDate],
                        [PaymentUser],
                        [PaymentStatus])
				VALUES(	@Id, 
                        GETDATE(), 
                        @CustomerId,
                        @BranchId, 
                        @No, 
                        @Description, 
                        @DeliveryDate, 
                        @DeliveryPointId, 
                        @DeliveryVehicleId, 
                        @Receiver, 
                        @ReceiverPhone, 
                        @ReceiverAddress, 
                        @DeliveryType,
                        @VoucherCode, 
                        @Status,
                        @IsShipping, 
                        @TotalQuantityReg, 
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
                        @PaymentDate,
                        @PaymentUser,
                        @PaymentStatus)			
		COMMIT
		SELECT @Id 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT '';
		THROW;
	END CATCH
END

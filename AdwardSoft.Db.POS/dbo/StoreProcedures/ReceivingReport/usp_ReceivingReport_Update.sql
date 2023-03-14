CREATE PROCEDURE [dbo].[usp_ReceivingReport_Update]
	@Id CHAR(32), 
    @Date DATE, 
    @SupplierId INT, 
    @BranchId INT, 
    @No CHAR(15), -- unique Regex: “RR“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7) Format: example: SR 01 20 12 0000001 → RR0120120000001
    @Description NVARCHAR(128), 
    @Status TINYINT, -- 0: Wating - Chờ duyệt, 1: Approved - Đã duyệt, 2 : Trash - Xoá
    @IsPurchaseOrder BIT, 
    @TotalQuantity NUMERIC(8, 3), 
    @SubTotal NUMERIC(16, 2), 
    @TaxRate NUMERIC(5, 2), 
    @TaxFee NUMERIC(16, 2), 
    @TotalDiscount NUMERIC(16, 2), 
    @TotalAmount NUMERIC(16, 2), 
    @PaymentMethodId INT, 
    @UserId BIGINT, 
    @CreateDate SMALLDATETIME, 
    @CreatedUser BIGINT, 
    @ModifiedDate SMALLDATETIME, 
    @ModifiedUser BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN	
                
				UPDATE  [dbo].[ReceivingReport] 
                 SET    [Date] = GETDATE(), 
                        [SupplierId] = @SupplierId, 
                        [Description] = @Description, 
                        [Status] = @Status,
                        [IsPurchaseOrder] = @IsPurchaseOrder, 
                        [TotalQuantity] = @TotalQuantity, 
                        [SubTotal] = @SubTotal, 
                        [TaxRate] = @TaxRate, 
                        [TaxFee] = @TaxFee, 
                        [TotalDiscount] = @TotalDiscount, 
                        [TotalAmount] = @TotalAmount, 
                        [PaymentMethodId] = @PaymentMethodId,  
                        [ModifiedDate] = GETDATE(), 
                        [ModifiedUser] = @ModifiedUser
				WHERE	[Id] = @Id		
		COMMIT
		SELECT 1 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

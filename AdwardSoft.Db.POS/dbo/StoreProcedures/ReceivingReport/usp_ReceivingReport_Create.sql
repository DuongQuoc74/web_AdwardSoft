CREATE PROCEDURE [dbo].[usp_ReceivingReport_Create]
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
                DECLARE @Index INT
                SET @Id = REPLACE(NEWID(), '-','');

                SELECT @Index = ISNULL(COUNT(s.[Id]), 0) + 1 
			    FROM [dbo].[ReceivingReport] s			
			    WHERE MONTH(s.CreateDate) = MONTH(GETDATE()) AND YEAR(s.CreateDate) = YEAR(GETDATE())

                SET @No = 'RR' + FORMAT(@SupplierId, 'd2') +   FORMAT(GETDATE(), 'yy') +  FORMAT(GETDATE(), 'MM') + FORMAT(@Index, 'd7')
				INSERT INTO [dbo].[ReceivingReport] 
                      ( [Id], 
                        [Date], 
                        [SupplierId], 
                        [BranchId], 
                        [No],
                        [Description], 
                        [Status],
                        [IsPurchaseOrder], 
                        [TotalQuantity], 
                        [SubTotal], 
                        [TaxRate], 
                        [TaxFee], 
                        [TotalDiscount], 
                        [TotalAmount], 
                        [PaymentMethodId], 
                        [UserId], 
                        [CreateDate], 
                        [CreatedUser], 
                        [ModifiedDate], 
                        [ModifiedUser])
				VALUES(	@Id, 
                        GETDATE(), 
                        @SupplierId, 
                        @BranchId, 
                        @No,
                        @Description, 
                        @Status,
                        @IsPurchaseOrder, 
                        @TotalQuantity, 
                        @SubTotal, 
                        @TaxRate, 
                        @TaxFee, 
                        @TotalDiscount, 
                        @TotalAmount, 
                        @PaymentMethodId, 
                        @UserId, 
                        GETDATE(), 
                        @CreatedUser, 
                        GETDATE(), 
                        @ModifiedUser )			
		COMMIT
		SELECT @Id 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT '';
		THROW;
	END CATCH
END

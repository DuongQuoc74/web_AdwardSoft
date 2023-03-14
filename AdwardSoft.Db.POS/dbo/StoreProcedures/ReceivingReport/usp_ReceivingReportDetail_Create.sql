CREATE PROCEDURE [dbo].[usp_ReceivingReportDetail_Create]
	@ReceivingReportDetailId CHAR(32), 
    @ReceivingReportId CHAR(32), 
    @ProductId INT, 
    @Quantity NUMERIC(8, 3), 
    @UnitId INT, 
    @Price NUMERIC(16, 2), 
    @Discount NUMERIC(16, 2), 
    @Amount NUMERIC(16, 2), 
    @IsPromo BIT,
    @ProductName NVARCHAR(50),
    @ProductCode NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN		
                SET @ReceivingReportDetailId = REPLACE(NEWID(), '-','');
				INSERT INTO [dbo].[ReceivingReportDetail] 
                      ( [ReceivingReportDetailId], 
                        [ReceivingReportId], 
                        [ProductId], 
                        [Quantity], 
                        [UnitId], 
                        [Price], 
                        [Discount], 
                        [Amount], 
                        [IsPromo])
				VALUES(	@ReceivingReportDetailId, 
                        @ReceivingReportId, 
                        @ProductId, 
                        @Quantity, 
                        @UnitId, 
                        @Price, 
                        @Discount, 
                        @Amount, 
                        @IsPromo )			
		COMMIT
		SELECT 1 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

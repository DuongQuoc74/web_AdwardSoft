CREATE PROCEDURE [dbo].[usp_SaleReceiptScore_Create]
	@SaleReceiptId CHAR(32), 
    @ScoreConfigurationId INT, 
    @Point NUMERIC(9), 
    @Amount NUMERIC(16, 2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[SaleReceiptScore] (	[SaleReceiptId], 
													[ScoreConfigurationId], 
													[Point], 
													[Amount] )
			VALUES(	@SaleReceiptId, 
                    @ScoreConfigurationId,
					@Point,
					@Amount )	                    		
		COMMIT	
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

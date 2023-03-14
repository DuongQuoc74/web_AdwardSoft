CREATE PROCEDURE [dbo].[usp_SaleReceiptScore_Delete]
	@SaleReceiptId CHAR(32), 
	@ScoreConfigurationId INT
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[SaleReceiptScore]
			WHERE [SaleReceiptId] = @SaleReceiptId AND [ScoreConfigurationId] = @ScoreConfigurationId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END
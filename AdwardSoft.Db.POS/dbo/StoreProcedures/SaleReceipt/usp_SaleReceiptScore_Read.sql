CREATE PROCEDURE [dbo].[usp_SaleReceiptScore_Read]
	@SaleReceiptId CHAR(32), 
	@ScoreConfigurationId INT
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			SELECT * FROM [dbo].[SaleReceiptScore]
			WHERE [SaleReceiptId] = @SaleReceiptId AND [ScoreConfigurationId] = @ScoreConfigurationId
		COMMIT	
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		THROW;
	END CATCH
END
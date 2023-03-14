CREATE PROCEDURE [dbo].[usp_BeginingStock_UpdateUnlock]
	@StockId INT,
	@Year CHAR(4)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[BeginingStock]
			SET [IsLock] = 0
			WHERE [StockId] = @StockId AND [Year] = @Year
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

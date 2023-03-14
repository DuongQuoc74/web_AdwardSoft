CREATE PROCEDURE [dbo].[usp_StockTaking_DeleteByDate]
	@StockId INT,
	@Date DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[Stocktaking]
			WHERE [StockId] = @StockId AND [Date] = (SELECT CONVERT(DATE, @Date))
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

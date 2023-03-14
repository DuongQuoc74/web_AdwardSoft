CREATE PROCEDURE [dbo].[usp_StockTaking_Delete]
	@ProductId INT,
	@StockId INT,
	@Date DATE,
	@UnitId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[Stocktaking]
			WHERE [ProductId] = @ProductId 
			AND [StockId] = @StockId 
			AND [UnitId] = @UnitId 
			AND [Date] = (SELECT CONVERT(DATE, @Date))
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

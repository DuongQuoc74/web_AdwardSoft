CREATE PROCEDURE [dbo].[usp_BeginingStock_Delete]
	@StockId INT,
	@ProductId INT,
	@Year CHAR(4),
	@UnitId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[BeginingStock]
			WHERE [StockId] = @StockId 
			AND [ProductId] = @ProductId
			AND [Year] = @Year
			AND [UnitId] = @UnitId
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

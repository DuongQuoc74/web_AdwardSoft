CREATE PROCEDURE [dbo].[usp_BeginingStock_UpdateQuantity]
	@Year CHAR(4),
	@StockId INT,
	@ProductId INT,
	@Quantity NUMERIC(13,3),
	@IsLock BIT,
	@UnitId BIT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[BeginingStock]
			SET [Quantity] = @Quantity
			WHERE [ProductId] = @ProductId 
			AND [StockId] = @StockId 
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

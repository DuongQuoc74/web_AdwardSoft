CREATE PROCEDURE [dbo].[usp_StockTaking_UpdateQuantity]
	@StockId INT, 
    @ProductId INT, 
    @Date DATETIME,
    @Quantity FLOAT,
	@IsLock BIT,
	@IsForward BIT,
	@UnitId  INT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Stocktaking]
			SET [Quantity] = @Quantity
			WHERE [ProductId] = @ProductId 
			AND [StockId] = @StockId 
			AND [UnitId] = @UnitId 
			AND CONVERT(DATE, [Date]) = CONVERT(DATE, @Date)
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_SellingPrice_Update]
	@ProductId INT,
	@UnitId INT,
	@Date DATE,
	@WholesalePrice NUMERIC(16,2),
	@RetailPrice NUMERIC(16,2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[SellingPrice]
			SET [WholesalePrice] = @WholesalePrice,
				[RetailPrice] = @RetailPrice
			WHERE [ProductId] = @ProductId AND [UnitId] = @UnitId AND [Date] = @Date
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

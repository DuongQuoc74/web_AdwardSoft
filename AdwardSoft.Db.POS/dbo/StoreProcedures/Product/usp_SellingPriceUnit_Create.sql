CREATE PROCEDURE [dbo].[usp_SellingPriceUnit_Create]
	@ProductId INT,
	@UnitId INT,
	@WholesalePrice NUMERIC(16,2),
	@RetailPrice NUMERIC(16,2),
	@QuantityFrom NUMERIC(13,3),
	@QuantityTo NUMERIC(13,3)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[ProductUnitConverter] WHERE [UnitId] = @UnitId AND [ProductId] = @ProductId)
			BEGIN
				INSERT INTO [dbo].[ProductUnitConverter]
				([ProductId], [UnitId], [QuantityFrom], [QuantityTo])
				VALUES(@ProductId, @UnitId, @QuantityFrom, @QuantityTo)
			END

			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[SellingPrice] WHERE [UnitId] = @UnitId AND [ProductId] = @ProductId AND CONVERT(DATE,[Date]) = CONVERT(DATE, GETDATE()))
			BEGIN
				INSERT INTO [dbo].[SellingPrice] 
				([ProductId], [UnitId], [Date], [WholesalePrice], [RetailPrice])
				VALUES(@ProductId, @UnitId, GETDATE(), @WholesalePrice, @RetailPrice)
			END
		COMMIT
		SELECT 1;
		RETURN 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		RETURN 0;
		THROW;
	END CATCH
END

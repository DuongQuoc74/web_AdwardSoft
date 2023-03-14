CREATE PROCEDURE [dbo].[usp_SellingPrice_Create]
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
			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[SellingPrice] WHERE [UnitId] = @UnitId AND [ProductId] = @ProductId AND CONVERT(DATE,[Date]) = CONVERT(DATE, GETDATE()))
			BEGIN
				INSERT INTO [dbo].[SellingPrice] ([ProductId], [UnitId], [Date], [WholesalePrice], [RetailPrice])
				VALUES(@ProductId, @UnitId, GETDATE(), @WholesalePrice, @RetailPrice)
			END
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

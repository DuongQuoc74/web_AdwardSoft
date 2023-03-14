CREATE PROCEDURE [dbo].[usp_BeginingStock_Create]
	@Year CHAR(4),
	@StockId INT,
	@ProductId INT,
	@Quantity NUMERIC(13,3),
	@IsLock BIT,
	@UnitId INT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF EXISTS (SELECT TOP 1 1 FROM [dbo].[BeginingStock] WHERE [ProductId] = @ProductId AND [StockId] = @StockId  AND [Year] = @Year AND [UnitId] = @UnitId)
			BEGIN
				UPDATE [dbo].[BeginingStock]
				SET [Quantity] = [Quantity] + @Quantity
				WHERE [ProductId] = @ProductId 
				AND [StockId] = @StockId 
				AND [Year] = @Year
				AND [UnitId] = @UnitId
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[BeginingStock] ([Year], [StockId],[ProductId],[Quantity],[IsLock], [UnitId], [UserId])
				VALUES(@Year, @StockId, @ProductId, @Quantity, @IsLock, @UnitId, @UserId)
			END

		COMMIT

		SELECT BT.* 
		FROM [dbo].[BeginingStock] AS BT
		WHERE BT.[StockId] = @StockId
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

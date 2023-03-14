CREATE PROCEDURE [dbo].[usp_StockTaking_Create]
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
			IF EXISTS (SELECT TOP 1 1 FROM [dbo].[Stocktaking] WHERE [ProductId] = @ProductId AND [StockId] = @StockId  AND CONVERT(DATE, [Date]) = CONVERT(DATE, @Date) AND [UnitId] = @UnitId)
			BEGIN
				UPDATE [dbo].[Stocktaking]
				SET [Quantity] = [Quantity] + @Quantity
				WHERE [ProductId] = @ProductId 
				AND [StockId] = @StockId 
				AND [UnitId] = @UnitId 
				AND CONVERT(DATE, [Date]) = CONVERT(DATE, @Date)
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Stocktaking] ([StockId],[ProductId],[Date],[Quantity],[IsLock],[IsForward],[UnitId],[UserId])
				VALUES(@StockId,  @ProductId, GETDATE(), @Quantity, @IsLock, @IsForward, @UnitId, @UserId)
			END

		COMMIT

		SELECT ST.* 
		FROM [dbo].[StockTaking] AS ST
		WHERE ST.[StockId] = @StockId
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

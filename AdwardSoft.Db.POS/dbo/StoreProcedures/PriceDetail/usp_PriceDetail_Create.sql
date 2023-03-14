CREATE PROCEDURE [dbo].[usp_PriceDetail_Create]
	@ProductId INT,
	@LocationId INT,
	@DeliveryPointId INT,
	@DeliveryType TINYINT,
	@Price NUMERIC(16, 2),
	@Date DATE
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[PriceDetail]
			([ProductId], [LocationId], [DeliveryPointId], [DeliveryType], [Price], [Date])
			VALUES  (@ProductId, @LocationId, @DeliveryPointId, @DeliveryType, @Price, @Date)
		COMMIT
		SELECT * 
		FROM [dbo].[PriceDetail]
		WHERE [ProductId] = @ProductId AND [DeliveryPointId] = @DeliveryPointId 
			AND [LocationId] = @LocationId AND [DeliveryType] = @DeliveryType AND [Date] = @Date
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END


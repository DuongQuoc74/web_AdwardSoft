CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_Update]
	@ProductId INT,
	@UnitId INT,
	@QuantityFrom NUMERIC(13,3),
	@QuantityTo NUMERIC(13,3)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[ProductUnitConverter]
			SET QuantityFrom = @QuantityFrom, 
				QuantityTo = @QuantityTo
			WHERE [ProductId] = @ProductId AND [UnitId] = @UnitId
		COMMIT
		SELECT P.*
		FROM [dbo].[ProductUnitConverter] AS P
		WHERE P.[ProductId] = @ProductId AND P.[UnitId] = @UnitId
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_Create]
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
			INSERT INTO [dbo].[ProductUnitConverter]
			([ProductId], [UnitId], [QuantityFrom], [QuantityTo])
			VALUES(@ProductId, @UnitId, @QuantityFrom, @QuantityTo)
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
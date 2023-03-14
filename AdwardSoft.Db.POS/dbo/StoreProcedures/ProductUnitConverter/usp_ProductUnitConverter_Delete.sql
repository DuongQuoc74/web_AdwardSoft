CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_Delete]
	@ProductId INT,
	@UnitId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[ProductUnitConverter]
			WHERE [ProductId] = @ProductId AND [UnitId] = @UnitId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0
		THROW;
	END CATCH
END
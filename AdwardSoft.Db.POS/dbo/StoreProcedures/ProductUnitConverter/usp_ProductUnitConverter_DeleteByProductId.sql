CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_DeleteByProductId]
	@ProductId INT
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[ProductUnitConverter]
			WHERE [ProductId] = @ProductId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

	
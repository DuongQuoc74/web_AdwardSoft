CREATE PROCEDURE [dbo].[usp_SellingPrice_Delete]
	@ProductId INT,
	@UnitId INT,
	@Date DATE
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[SellingPrice]
			WHERE [UnitId] = @UnitId AND [ProductId] = @ProductId AND [Date] = @Date
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

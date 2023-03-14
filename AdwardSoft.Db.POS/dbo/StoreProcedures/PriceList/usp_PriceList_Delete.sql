CREATE PROCEDURE [dbo].[usp_PriceList_Delete]
	@Date DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[PriceList]
			WHERE [Date] = @Date

			DELETE [dbo].[PriceDetail]
			WHERE [Date] = @Date

			SELECT 1;
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

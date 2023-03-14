CREATE PROCEDURE [dbo].[usp_ReceivingReportDetail_Delete]
	@ReceivingReportId CHAR(32)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[ReceivingReportDetail]
			WHERE [ReceivingReportId] = @ReceivingReportId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

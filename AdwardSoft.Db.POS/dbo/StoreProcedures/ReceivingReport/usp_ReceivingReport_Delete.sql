CREATE PROCEDURE [dbo].[usp_ReceivingReport_Delete]
	@Id CHAR(32)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[ReceivingReport]
			WHERE [Id] = @Id

			DELETE [dbo].[ReceivingReportDetail]
			WHERE [ReceivingReportId] = @Id
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

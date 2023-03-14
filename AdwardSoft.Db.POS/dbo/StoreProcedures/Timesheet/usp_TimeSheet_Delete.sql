CREATE PROCEDURE [dbo].[usp_TimeSheet_Delete]
	@UserId BIGINT, @Date DATE
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[TimeSheet]
			WHERE [UserId] = @UserId AND [Date] = @Date

		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

CREATE PROCEDURE [dbo].[usp_EmployeeUser_Delete]
	@EmployeeId INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[EmployeeUser]
			WHERE [EmployeeId] = @EmployeeId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

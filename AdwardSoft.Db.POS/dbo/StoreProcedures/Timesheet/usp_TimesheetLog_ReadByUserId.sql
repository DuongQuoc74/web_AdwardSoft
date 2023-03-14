CREATE PROCEDURE [dbo].[usp_TimesheetLog_ReadByUserId]
	@UserId BIGINT 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[TimesheetLog] p
		WHERE p.[UserId] = @UserId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

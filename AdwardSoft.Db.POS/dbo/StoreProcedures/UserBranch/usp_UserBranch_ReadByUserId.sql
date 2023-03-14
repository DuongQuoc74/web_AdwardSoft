CREATE PROCEDURE [dbo].[usp_UserBranch_ReadByUserId]
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[UserBranch]
		WHERE [UserId] = @UserId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

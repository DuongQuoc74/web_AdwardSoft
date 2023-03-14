CREATE PROCEDURE [dbo].[usp_UserBranch_Create]
    @UserId BIGINT, 
    @BranchId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[UserBranch] ([UserId], [BranchId])
			VALUES (@UserId, @BranchId)
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

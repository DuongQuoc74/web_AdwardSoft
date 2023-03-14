CREATE PROCEDURE [dbo].[usp_EmployeeUser_Create]
	@EmployeeId INT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[EmployeeUser]
			WHERE [EmployeeId] = @EmployeeId

			INSERT INTO [dbo].[EmployeeUser] ([EmployeeId], [UserId])
			VALUES(@EmployeeId, @UserId)
		COMMIT
		SELECT 1
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		RETURN 0;
		THROW;
	END CATCH
END

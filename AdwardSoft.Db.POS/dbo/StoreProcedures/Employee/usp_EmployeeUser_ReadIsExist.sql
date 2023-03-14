CREATE PROCEDURE [dbo].[usp_EmployeeUser_ReadIsExist]
	@EmployeeId INT,
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS ( SELECT TOP 1 1 FROM [dbo].[EmployeeUser] p
						WHERE p.[EmployeeId] <> @EmployeeId 
						AND p.[UserId] = @UserId)
				SELECT 0
			ELSE 
				SELECT 1
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

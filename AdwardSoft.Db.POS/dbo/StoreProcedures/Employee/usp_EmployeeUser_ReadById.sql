CREATE PROCEDURE [dbo].[usp_EmployeeUser_ReadById]
	@EmployeeId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[EmployeeUser]
		WHERE [EmployeeId] = @EmployeeId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

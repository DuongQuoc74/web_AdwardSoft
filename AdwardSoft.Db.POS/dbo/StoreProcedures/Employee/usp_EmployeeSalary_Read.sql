CREATE PROCEDURE [dbo].[usp_EmployeeSalary_Read]
	@EmployeeId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[EmployeeSalary]
		WHERE [EmployeeId] = @EmployeeId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

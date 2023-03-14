CREATE PROCEDURE [dbo].[usp_Employee_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[EmployeeUser]
			WHERE [EmployeeId] = @Id

			DELETE [dbo].[EmployeeSalary]
			WHERE [EmployeeId] = @Id

			DELETE [dbo].[EmployeeShift]
			WHERE [EmployeeId] = @Id

			DELETE [dbo].[Employee]
			WHERE [Id] = @Id
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

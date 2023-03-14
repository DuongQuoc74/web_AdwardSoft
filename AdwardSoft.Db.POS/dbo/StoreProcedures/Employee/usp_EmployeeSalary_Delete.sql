CREATE PROCEDURE [dbo].[usp_EmployeeSalary_Delete]
	@EmployeeId INT,
	@EffectiveDate DATE
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[EmployeeSalary]
			WHERE [EmployeeId] = @EmployeeId AND [EffectiveDate] = @EffectiveDate
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END
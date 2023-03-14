CREATE PROCEDURE [dbo].[usp_EmployeeSalary_ReadById]
	@EmployeeId INT,
	@EffectiveDate DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT TOP 1 *
		FROM [dbo].[EmployeeSalary]
		WHERE [EmployeeId] = @EmployeeId AND [EffectiveDate] = @EffectiveDate
		ORDER BY [EffectiveDate] DESC
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

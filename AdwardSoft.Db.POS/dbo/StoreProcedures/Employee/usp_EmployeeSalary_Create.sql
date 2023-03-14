CREATE PROCEDURE [dbo].[usp_EmployeeSalary_Create]
	@EmployeeId INT, 
    @EffectiveDate DATE, 
    @BasicSalary NUMERIC(16, 2), 
    @Type TINYINT, -- 0 : Hourly wages , 1 : Day’s wages , 2 : Fixed wages, 3 : Contractual wages
    @Wage TINYINT, --0: Net, 1: Gross → Hình thức lương
    @ActualWage NUMERIC(16, 2) -- → Lương thực nhận
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[EmployeeSalary]
			WHERE [EmployeeId] = @EmployeeId AND CONVERT(DATE, GETDATE()) = [EffectiveDate])
			BEGIN
				INSERT INTO [dbo].[EmployeeSalary] ([EmployeeId],[EffectiveDate],[BasicSalary],[Type],[Wage],[ActualWage])
				VALUES(@EmployeeId, @EffectiveDate, @BasicSalary, @Type, @Wage, @ActualWage)
			END
			ELSE
			BEGIN
				UPDATE [dbo].[EmployeeSalary]
				SET [BasicSalary] = @BasicSalary,
					[Type] = @Type,
					[Wage] = @Wage,
					[ActualWage] = @ActualWage
				WHERE [EmployeeId] = @EmployeeId AND CONVERT(DATE, GETDATE()) = [EffectiveDate]
			END
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

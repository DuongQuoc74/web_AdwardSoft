CREATE TABLE [dbo].[EmployeeSalary]
(
	[EmployeeId] INT NOT NULL , 
    [EffectiveDate] DATE NOT NULL, 
    [BasicSalary] NUMERIC(16, 2) NOT NULL, 
    [Type] TINYINT NOT NULL,--0: Net, 1: Gross → Hình thức lương
    [Wage] TINYINT NOT NULL,  -- 0 : Hourly wages , 1 : Day’s wages , 2 : Fixed wages, 3 : Contractual wages
    [ActualWage] NUMERIC(16, 2) NOT NULL, -- → Lương thực nhận 
    PRIMARY KEY ([EmployeeId], [EffectiveDate])
)

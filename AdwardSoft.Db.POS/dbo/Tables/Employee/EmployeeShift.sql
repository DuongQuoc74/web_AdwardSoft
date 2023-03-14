CREATE TABLE [dbo].[EmployeeShift]
(
	[EmployeeId] INT NOT NULL , 
    [ShiftId] INT NOT NULL, 
    [Month] NUMERIC(2) NOT NULL, 
    [Year] NUMERIC(4) NOT NULL, 
    [Type] TINYINT NOT NULL, -- 0 Shop Assistant , 1 Cashier
    CONSTRAINT [FK_EmployeeShift_Shift] FOREIGN KEY ([ShiftId]) REFERENCES [Shift]([Id]), 
    CONSTRAINT [FK_EmployeeShift_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee]([Id]), 
    PRIMARY KEY ([EmployeeId], [ShiftId], [Month], [Year])
)

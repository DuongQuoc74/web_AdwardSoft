CREATE TABLE [dbo].[EmployeeCheckoutCounter]
(
	[EmployeeId] INT NOT NULL , 
    [ShiftId] INT NOT NULL, 
    [Month] NUMERIC(2) NOT NULL, 
    [Year] NUMERIC(4) NOT NULL, 
    [CheckoutCounterId] INT NOT NULL, 
    PRIMARY KEY ([EmployeeId], [ShiftId], [Month], [Year], [CheckoutCounterId])
)

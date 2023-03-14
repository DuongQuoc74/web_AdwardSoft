CREATE TABLE [dbo].[EmployeeUser]
(
	[EmployeeId] INT NOT NULL , 
    [UserId] BIGINT NOT NULL, 
    PRIMARY KEY ([EmployeeId], [UserId]), 
    CONSTRAINT [FK_EmployeeUser_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee]([Id])
)

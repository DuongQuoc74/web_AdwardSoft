CREATE TABLE [dbo].[Shift]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [IsMonday] BIT NOT NULL, 
    [IsTuesday] BIT NOT NULL, 
    [IsWednesday] BIT NOT NULL, 
    [IsThursday] BIT NOT NULL, 
    [IsFriday] BIT NOT NULL, 
    [IsSaturday] BIT NOT NULL, 
    [IsSunday] BIT NOT NULL, 
    [StartTime] CHAR(5) NOT NULL, 
    [EndTime] CHAR(5) NOT NULL, 
    [BranchId] INT NOT NULL, 
    CONSTRAINT [FK_Shift_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id])
)

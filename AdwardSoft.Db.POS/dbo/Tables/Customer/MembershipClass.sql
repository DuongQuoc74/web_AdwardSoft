CREATE TABLE [dbo].[MembershipClass]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(60) NOT NULL, 
    [LowestValue] NUMERIC(16, 2) NOT NULL, 
    [HighestValue] NUMERIC(16, 2) NOT NULL, 
    [Level] TINYINT NOT NULL, 
    [IsDefault] BIT NOT NULL
)

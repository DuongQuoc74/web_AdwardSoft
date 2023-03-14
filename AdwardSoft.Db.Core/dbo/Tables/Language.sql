CREATE TABLE [dbo].[Language]
(
	[Code] CHAR(2) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [IsDefault] BIT NULL
)

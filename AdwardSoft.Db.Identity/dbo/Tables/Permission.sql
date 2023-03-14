CREATE TABLE [dbo].[Permission]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Description] NVARCHAR(128) NOT NULL,
	[Controller] VARCHAR(50) NOT NULL, 
    [Action] VARCHAR(20) NOT NULL
)

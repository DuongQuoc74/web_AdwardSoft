CREATE TABLE [dbo].[Department]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(80) NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    [Sort] INT NOT NULL, 
    [ParentId] INT NOT NULL
)

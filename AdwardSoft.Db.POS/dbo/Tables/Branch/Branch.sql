CREATE TABLE [dbo].[Branch]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(120) NOT NULL, 
    [Address] NVARCHAR(128) NOT NULL, 
    [Tel] VARCHAR(12) NOT NULL, 
    [Email] VARCHAR(254) NOT NULL, 
    [ParentId] INT NOT NULL, 
    [Sort] INT NULL
)

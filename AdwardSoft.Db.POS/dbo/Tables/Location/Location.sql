CREATE TABLE [dbo].[Location]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Code] VARCHAR(6) NOT NULL UNIQUE,
    [Name] NVARCHAR(120) NOT NULL, 
    [PostalCode] VARCHAR(10) NOT NULL, 
    [Rate] NUMERIC(3) DEFAULT 0, 
    [ParentId] INT, 
    [Status] TINYINT DEFAULT 1,
    CONSTRAINT [FK_Location] FOREIGN KEY ([ParentId]) REFERENCES [Location]([Id])
)

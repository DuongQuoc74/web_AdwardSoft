CREATE TABLE [dbo].[Category]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ParentId] INT NOT NULL, 
    CONSTRAINT [FK_Category_Category] FOREIGN KEY ([ParentId]) REFERENCES [Category]([Id])
)

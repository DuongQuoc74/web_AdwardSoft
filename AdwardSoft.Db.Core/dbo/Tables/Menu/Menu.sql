CREATE TABLE [dbo].[Menu]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ParentId] INT NOT NULL, 
    [Order] INT NOT NULL, 
    [Type] TINYINT NOT NULL, --0: Page, 1: Post, 2: Custom, 3: Category 
	[MenuGroupId] INT NOT NULL,
    [ReferenceId] INT NOT NULL, 
    CONSTRAINT [FK_Menu_MenuStructure] FOREIGN KEY ([ParentId]) REFERENCES [Menu]([Id]), 
    CONSTRAINT [FK_Menu_MenuGroup] FOREIGN KEY ([MenuGroupId]) REFERENCES [MenuGroup]([Id])
)

GO

CREATE INDEX [IX_Menu_MenuGroupId] ON [dbo].[Menu] ([MenuGroupId])

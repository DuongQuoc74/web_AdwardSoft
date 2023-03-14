CREATE TABLE [dbo].[Tag]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [LanguageCode] CHAR(2) NOT NULL, 
    [Name] NVARCHAR(35) NOT NULL, 
    [Slug] VARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    CONSTRAINT [FK_Tag_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language]([Code])
)

GO

CREATE INDEX [IX_Tag_LanguageCode] ON [dbo].[Tag] ([LanguageCode])

CREATE TABLE [dbo].[CategoryTranslation]
(
	[CategoryId] INT NOT NULL , 
    [LanguageCode] CHAR(2) NOT NULL, 
    [Name] NVARCHAR(70) NOT NULL, 
    [Slug] VARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    PRIMARY KEY ([LanguageCode], [CategoryId]), 
    CONSTRAINT [FK_CategoryTranslation_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id]), 
    CONSTRAINT [FK_CategoryTranslation_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language]([Code])
)

GO

CREATE INDEX [IX_CategoryTranslation_LanguageCode] ON [dbo].[CategoryTranslation] ([LanguageCode])

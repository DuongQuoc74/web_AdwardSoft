CREATE TABLE [dbo].[MenuTranslation]
(
	[MenuId] INT NOT NULL , 
    [LanguageCode] CHAR(2) NOT NULL, 
    [URL] VARCHAR(2048) NOT NULL, 
    [NavigationLabel] NVARCHAR(70) NOT NULL, 
    PRIMARY KEY ([MenuId], [LanguageCode]), 
    CONSTRAINT [FK_MenuTranslation_MenuStructure] FOREIGN KEY ([MenuId]) REFERENCES [Menu]([Id]), 
    CONSTRAINT [FK_MenuTranslation_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language]([Code])
)

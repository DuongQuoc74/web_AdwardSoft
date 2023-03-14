CREATE TABLE [dbo].[PageTranslation]
(
	[PageId] INT NOT NULL , 
    [LanguageCode] CHAR(2) NOT NULL, 
    [Title] NVARCHAR(150) NOT NULL, 
	[Content] NVARCHAR(MAX) NOT NULL, 
    [Permalink] VARCHAR(512) NOT NULL, 
    PRIMARY KEY ([PageId], [LanguageCode]), 
    CONSTRAINT [FK_PageTranslation_Page] FOREIGN KEY ([PageId]) REFERENCES [Page]([Id]), 
    CONSTRAINT [FK_PageTranslation_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language]([Code])
)

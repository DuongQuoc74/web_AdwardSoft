CREATE TABLE [dbo].[PageSocial]
(
	[PageId] INT NOT NULL , 
    [LanguageCode] CHAR(2) NOT NULL, 
	[SocialId] INT NOT NULL,
    [Title] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(300) NOT NULL, 
    [Image] VARCHAR(2048) NOT NULL, 
    PRIMARY KEY ([PageId], [SocialId], [LanguageCode]), 
    CONSTRAINT [FK_PageSocial_Page] FOREIGN KEY ([PageId]) REFERENCES [Page]([Id]), 
    CONSTRAINT [FK_PageSocial_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language]([Code]) 
)

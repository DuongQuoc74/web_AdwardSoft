CREATE TABLE [dbo].[HandleErrorTrans]
(
	[HandleErrorId] INT NOT NULL , 
    [LanguageCode] CHAR(2) NOT NULL, 
    [Title] NVARCHAR(150) NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([HandleErrorId], [LanguageCode])
)

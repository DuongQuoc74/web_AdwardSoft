CREATE TABLE [dbo].[Post]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Status] TINYINT NOT NULL, --0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
    [Visibility] TINYINT NOT NULL, --0: Public, 1: Password Protected, 2: Private
    [IsStick] BIT NOT NULL, -- Only visible > Visibility = Public
    [PasswordProtected] VARCHAR(60) NULL, -- Only visible > Visibility = Password Protected
    [PublishedOn] SMALLDATETIME NOT NULL, 
    [Format] TINYINT NOT NULL, --0: Standard, 1: Image, 2: Video, 3: Quote, 4: Gallery
    [FeaturedImage] VARCHAR(2048) NOT NULL, 
	[Title] NVARCHAR(150) NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    [Content] NVARCHAR(MAX) NOT NULL, 
    [Permalink] VARCHAR(512) NOT NULL, 
    [IsMenuLink] BIT NOT NULL 
)

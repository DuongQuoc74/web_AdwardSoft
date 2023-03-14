CREATE TABLE [dbo].[Page]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Status] TINYINT NOT NULL, --0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
    [Visibility] TINYINT NOT NULL, --0: Public, 1: Password Protected, 2: Private
	[PasswordProtected] VARCHAR(60) NULL, -- Only visible > Visibility = Password Protected
	[PublishedOn] SMALLDATETIME NOT NULL, 
    [FeaturedImage] VARCHAR(2048) NOT NULL, 
    [UserId] BIGINT NOT NULL
)

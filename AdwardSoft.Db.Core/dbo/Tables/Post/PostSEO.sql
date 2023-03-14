CREATE TABLE [dbo].[PostSEO]
(
	[PostId] INT NOT NULL , 
    [Title] NVARCHAR(71) NOT NULL, 
    [Description] NVARCHAR(160) NOT NULL, 
    [CanonicalURL] VARCHAR(2048) NULL, 
	[MetaRobotIndex]    TINYINT        NOT NULL, --1: Index, 0: NoIndex
    [MetaRobotFollow]   TINYINT        NOT NULL, --1: Follow, 0: NoFollow
    [MetaRobotAdvanced] TINYINT        NOT NULL, --0: None, 1: NO ODP, 2: NO YDIR, 3: No Archive, 4: No Snippet
    PRIMARY KEY ([PostId]), 
    CONSTRAINT [FK_PostSEO_Post] FOREIGN KEY ([PostId]) REFERENCES [Post]([Id]), 
)

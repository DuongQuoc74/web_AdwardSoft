CREATE TABLE [dbo].[PostGallery]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
    [PostId] INT NOT NULL, 
    [Image] VARCHAR(2048) NOT NULL, 
    CONSTRAINT [FK_PostGallery_ToTable] FOREIGN KEY ([PostId]) REFERENCES [Post]([Id])
)

GO

CREATE INDEX [IX_PostGallery_PostId] ON [dbo].[PostGallery] ([PostId])

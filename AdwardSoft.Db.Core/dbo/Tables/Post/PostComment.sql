CREATE TABLE [dbo].[PostComment]
(
	[Id] CHAR(32) NOT NULL , 
    [PostId] BIGINT NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [Date] SMALLDATETIME NULL, 
    [ParentId] CHAR(32) NOT NULL, 
    [Comment] NVARCHAR(300) NOT NULL, 
    [Status] TINYINT NULL, -- 0: Pending, 1: Published , 2: Mark as Spam 
    PRIMARY KEY ([ParentId], [PostId], [UserId], [Id]), 
    --CONSTRAINT [FK_PostComment_PostComment] FOREIGN KEY ([ParentId]) REFERENCES [PostComment]([Id])
)

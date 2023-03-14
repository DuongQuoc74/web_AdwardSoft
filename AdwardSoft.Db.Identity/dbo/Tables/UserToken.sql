CREATE TABLE [dbo].[UserToken]
(
	[UserId] BIGINT NOT NULL , 
    [LoginProvider] NVARCHAR(128) NOT NULL, 
    [Name] NVARCHAR(128) NOT NULL, 
    [Value] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_UserToken_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [PK_UserToken] PRIMARY KEY ([UserId])
)

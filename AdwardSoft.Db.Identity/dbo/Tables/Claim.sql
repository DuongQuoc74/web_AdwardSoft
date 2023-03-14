CREATE TABLE [dbo].[Claim]
(
	[Id] INT NOT NULL , 
    [UserId] BIGINT NOT NULL, 
    [ClaimType] NVARCHAR(MAX) NULL, 
    [ClaimValue] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Claim_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [PK_Claim] PRIMARY KEY ([Id])
)

GO

CREATE INDEX [IX_Claim_UserId] ON [dbo].[Claim] ([UserId])

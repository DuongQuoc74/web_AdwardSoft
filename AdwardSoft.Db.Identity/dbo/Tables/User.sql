CREATE TABLE [dbo].[User]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL, 
    [UserName] VARCHAR(256) NOT NULL, 
	[NormalizedUserName] VARCHAR(256) NULL,
    [Email] VARCHAR(256) NOT NULL, 
	[NormalizedEmail] VARCHAR(256) NULL, 
    [EmailConfirmed] BIT NOT NULL , 
    [PasswordHash] NVARCHAR(MAX) NOT NULL, 
    [SecurityStamp] NVARCHAR(MAX) NOT NULL, 
	[ConcurrencyStamp] NVARCHAR(MAX) NULL, 
    [PhoneNumber] VARCHAR(50) NOT NULL, 
    [PhoneNumberConfirmed] BIT NOT NULL, 
    [TwoFactorEnabled] BIT NOT NULL, 
    [LockoutEndDateUtc] DATETIME NULL, 
    [LockoutEnabled] BIT NOT NULL, 
    [AccessFailedCount] INT NOT NULL, 
	[FullName] NVARCHAR(128) NOT NULL,
    [Avatar] VARCHAR(256) NULL,  
    [Type] INT NULL, -- 1 : Internal, 2 : Customer, 3 : Partner, 4 : Provider, 5 : Client , 6: Collaborators, 7: birth Certificate
    [Status] INT NULL, -- 0 : Available , 1 : Disable , 2 : Lock , 3 : Remove
    [UserIdx] INT NULL, 
    [IdentityNumber] varchar(12) NULL,
    [Gender] TINYINT NULL, -- 0 : Male, 1 : Female, 2 : None 
    PRIMARY KEY([Id])
)

GO

CREATE INDEX [IX_User_Email] ON [dbo].[User] ([Email])

GO

CREATE UNIQUE INDEX [IX_User_UserName] ON [dbo].[User] ([NormalizedUserName]) WHERE ([NormalizedUserName] IS NOT NULL)

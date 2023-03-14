CREATE TABLE [dbo].[NotificationTemplate]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Type] TINYINT NOT NULL, -- 0: Email, 1: SMS, 2: Mobile
    [Action] TINYINT NOT NULL, 
	[Title] NVARCHAR(150) NOT NULL, 
	[Content] NVARCHAR(4000) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [MailServerId] INT NOT NULL, 
	CONSTRAINT FK_NotificationTemplate_MailServer FOREIGN KEY (MailServerId) REFERENCES MailServer(Id)
)

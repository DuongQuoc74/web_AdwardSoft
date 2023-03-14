﻿CREATE TABLE [dbo].[MailServer]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Email] VARCHAR(2048) UNIQUE NOT NULL, 
    [Name] NVARCHAR(150) NOT NULL, 
    [SMTP] VARCHAR(120) NOT NULL, 
    [IsSSL] BIT NOT NULL DEFAULT 0, 
    [IsDefaultCredential] BIT NOT NULL DEFAULT 0,
    [Port] INT NOT NULL,
    [Pwd] VARCHAR(32) NOT NULL
)

CREATE TABLE [dbo].[BankAccount]
(
 [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
 [BankNo] VARCHAR(20) NOT NULL UNIQUE,
 [BankName] NVARCHAR(120) NOT NULL,
 [Status] TINYINT DEFAULT 1
 --0: Availble
 --1: Unavaible
)
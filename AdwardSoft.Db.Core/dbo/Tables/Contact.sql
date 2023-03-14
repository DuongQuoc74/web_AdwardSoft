CREATE TABLE [dbo].[Contact]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Date] SMALLDATETIME NULL, 
    [Name] NVARCHAR(70) NULL, 
    [Email] VARCHAR(256) NULL, 
    [Phone] VARCHAR(12) NULL, 
    [Message] NVARCHAR(300) NULL, 
    [Status] TINYINT NULL -- 0: Liên hệ mới, 1: Đã liên hệ, 2: Chưa thể liên lạc
)

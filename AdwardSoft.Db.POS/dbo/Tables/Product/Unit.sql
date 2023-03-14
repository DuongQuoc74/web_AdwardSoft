CREATE TABLE [dbo].[Unit]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    Code char(10) NOT NULL UNIQUE,
    [Name] NVARCHAR(10) NOT NULL, 
    [Status] TINYINT NOT NULL --0: Ngừng hoạt động , 1: Đang hoạt động
)

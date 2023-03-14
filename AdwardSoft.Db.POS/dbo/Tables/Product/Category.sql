CREATE TABLE [dbo].[Category]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(60) NOT NULL, 
    [Description] NVARCHAR(100) NOT NULL, 
    [Status] TINYINT NOT NULL DEFAULT 0 -- 0: Ngừng hoạt động,  1: Đang hoạt động
)

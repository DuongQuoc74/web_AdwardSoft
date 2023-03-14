CREATE TABLE [dbo].[PriceList]
(
	[Date] DATE NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(150) NOT NULL, 
    [Note] NVARCHAR(300) NULL, 
    [Status] TINYINT NOT NULL DEFAULT 0  --1: Hoạt động, 0: Không khả dụng, 2: Chờ duyệt
)

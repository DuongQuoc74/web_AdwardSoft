CREATE TABLE [dbo].[CustomerGroup]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(60) NOT NULL, 
    [Note] NVARCHAR(100) NULL, 
    [PricingPolicy] TINYINT NOT NULL, -- 0 : Wholesale Price (Giá bán sỉ), 1 : Retail Price (Giá bán lẻ)
    [Status] TINYINT NOT NULL -- 0 : Unavailable (Ngừng hoạt động) , 1 : Available (Đang hoạt động)
)

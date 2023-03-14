CREATE TABLE [dbo].[Product]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Code] CHAR(13) NOT NULL UNIQUE, 
    [Name] NVARCHAR(120) NOT NULL, 
    [Image] VARCHAR(2048) NULL, 
    [MinStock] NUMERIC(13, 3) NULL DEFAULT 1, 
    [MaxStock] NUMERIC(13, 3) NULL DEFAULT 1, 
    [UnitId] INT NOT NULL, 
    [StockId] INT NOT NULL, 
    [Status] TINYINT NOT NULL DEFAULT 0, -- 0: Mới tạo, 1: Hiển thị, 2: Tạm dừng, 3: Xóa
    [PriceType] TINYINT NOT NULL, -- Kiểu tính giá: 0: Tính theo lượng đăng ký, 1: Tính theo lượng thực giao
    [IsTrade] BIT NOT NULL, --Cho phép giao dịch
    [Description] NVARCHAR(300) NULL, 
    [Profit] NUMERIC(5, 3) NOT NULL, 
    [ModifyDate] SMALLDATETIME NOT NULL,
    CONSTRAINT [FK_Product_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]), 
    CONSTRAINT [FK_Product_Inventory] FOREIGN KEY ([StockId]) REFERENCES [Stock]([Id])
)
GO

CREATE UNIQUE INDEX [IX_Product_Code] ON [dbo].[Product] ([Code])

CREATE TABLE [dbo].[SaleReturnDetail]
(
	[SaleReturnDetailId] CHAR(32) NOT NULL PRIMARY KEY, 
    [SaleReturnId] CHAR(32) NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] NUMERIC(8, 3) NOT NULL, 
    [UnitId] INT NOT NULL, 
    [Price] NUMERIC(16, 2) NOT NULL, 
    [Discount] NUMERIC(16, 2) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [IsPromo] BIT NOT NULL, 
    CONSTRAINT [FK_SaleReturnDetail_SaleReturn] FOREIGN KEY ([SaleReturnId]) REFERENCES [SaleReturn]([Id]), 
    CONSTRAINT [FK_SaleReturnDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_SaleReturnDetail_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id])
)

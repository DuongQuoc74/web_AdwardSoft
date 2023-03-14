CREATE TABLE [dbo].[BeginingStock]
(
	[StockId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    [Year] CHAR(4) NOT NULL, 
    [Quantity] NUMERIC(13, 3) NULL, 
    [IsLock] BIT NULL, 
    [UnitId] INT NOT NULL, 
    [UserId] BIGINT NULL, 
    CONSTRAINT [FK_BeginingStock_Stock] FOREIGN KEY ([StockId]) REFERENCES [Stock]([Id]), 
    CONSTRAINT [FK_BeginingStock_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_BeginingStock_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]),
    PRIMARY KEY ([StockId], [ProductId], [Year], [UnitId])
)

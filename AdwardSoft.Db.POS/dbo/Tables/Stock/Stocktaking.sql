CREATE TABLE [dbo].[Stocktaking]
(
	[ProductId] INT NOT NULL, 
    [StockId] INT NOT NULL, 
    [Date] SMALLDATETIME NOT NULL, 
    [Quantity] NUMERIC(13, 3) NULL, 
    [IsLock] BIT NULL, 
    [IsForward] BIT NULL,
    [UnitId] INT NOT NULL, 
    [UserId] BIGINT NULL, 
    CONSTRAINT [FK_Stocktaking_Stock] FOREIGN KEY ([StockId]) REFERENCES [Stock]([Id]), 
    CONSTRAINT [FK_Stocktaking_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [PK_Stocktaking] PRIMARY KEY ([ProductId], [StockId], [Date], [UnitId]), 
    CONSTRAINT [FK_Stocktaking_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id])
)

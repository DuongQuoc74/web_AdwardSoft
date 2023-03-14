CREATE TABLE [dbo].[CustomerOrderDetails]
(
	[OrderId] CHAR(32) NOT NULL , 
    [ProductId] INT NOT NULL, 
    [QuantityReg] NUMERIC(8, 3) NOT NULL, 
    [Quantity] NUMERIC(8, 3) NOT NULL, 
    [UnitId] INT NOT NULL, 
    [Price] NUMERIC(16, 2) NOT NULL, 
    [Discount] NUMERIC(16, 2) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [IsPromo] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY ([OrderId], [ProductId]), 
    CONSTRAINT [FK_CustomerOrderDetails_CustomerOrder] FOREIGN KEY ([OrderId]) REFERENCES [CustomerOrder]([Id]), 
    CONSTRAINT [FK_CustomerOrderDetails_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_CustomerOrderDetails_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id])
)

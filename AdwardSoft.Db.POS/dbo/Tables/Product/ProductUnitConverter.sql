CREATE TABLE [dbo].[ProductUnitConverter]
(
	[ProductId] INT NOT NULL, 
    [UnitId] INT NOT NULL, 
    [QuantityFrom] NUMERIC(13, 3) NOT NULL, 
    [QuantityTo] NUMERIC(13, 3) NOT NULL

    PRIMARY KEY([ProductId],[UnitId])

    CONSTRAINT [FK_ProductUnitConverter_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]), 
    CONSTRAINT [FK_ProductUnitConverter_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)

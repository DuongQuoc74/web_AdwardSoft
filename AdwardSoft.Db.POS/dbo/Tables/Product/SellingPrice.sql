CREATE TABLE [dbo].[SellingPrice]
(
	[ProductId] INT NOT NULL , 
	[UnitId] INT NOT NULL , 
    [Date] DATE NOT NULL, --dd/MM/yyyy
    [WholesalePrice] NUMERIC(16, 2) NOT NULL, 
    [RetailPrice] NUMERIC(16, 2) NOT NULL,
    PRIMARY KEY ([ProductId], [UnitId], [Date]), 
    CONSTRAINT [FK_SellingPrice_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_SellingPrice_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id])
)

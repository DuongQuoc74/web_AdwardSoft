CREATE TABLE [dbo].[PurchaseOrderDetail]
(
	[PurchaseOrderId] CHAR(32) NOT NULL , 
    [ProductId] INT NOT NULL, 
    [Quantity] NUMERIC(8, 3) NOT NULL, 
    [UnitId] INT NOT NULL, 
    [Price] NUMERIC(16, 2) NOT NULL, 
    [PromotionId] INT NOT NULL, 
    [Discount] NUMERIC(16, 2) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    PRIMARY KEY ([PurchaseOrderId], [ProductId]), 
    CONSTRAINT [FK_PurchaseOrderDetail_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrder]([Id]), 
    CONSTRAINT [FK_PurchaseOrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_PurchaseOrderDetail_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]), 
    CONSTRAINT [FK_PurchaseOrderDetail_Promotion] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion]([Id])
)

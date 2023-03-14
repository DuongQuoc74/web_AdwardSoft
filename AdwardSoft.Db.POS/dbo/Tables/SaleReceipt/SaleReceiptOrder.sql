CREATE TABLE [dbo].[SaleReceiptOrder]
(
	[SaleReceiptId] CHAR(32) NOT NULL , 
    [CustomerOrderId] CHAR(32) NOT NULL, 
    PRIMARY KEY ([SaleReceiptId], [CustomerOrderId]), 
    CONSTRAINT [FK_SaleReceiptOrder_SaleReceipt] FOREIGN KEY ([SaleReceiptId]) REFERENCES [SaleReceipt]([Id]), 
    CONSTRAINT [FK_SaleReceiptOrder_CustomerOrder] FOREIGN KEY ([CustomerOrderId]) REFERENCES [CustomerOrder]([Id])
)

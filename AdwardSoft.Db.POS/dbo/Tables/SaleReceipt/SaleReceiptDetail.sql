CREATE TABLE [dbo].[SaleReceiptDetail]
(
    [SaleReceiptDetailId] CHAR(32) NOT NULL,
	[SaleReceiptId] CHAR(32) NOT NULL , 
    [ProductId] INT NOT NULL, 
    [Quantity] NUMERIC(13, 3) NOT NULL, 
    [UnitId] INT NOT NULL, 
    [Price] NUMERIC(16, 2) NOT NULL, 
    [Discount] NUMERIC(16, 2) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [IsPromo] BIT NOT NULL, [RetailPrice] NUMERIC(16, 2) NOT NULL, 
    [CardPinCode] VARCHAR(50) NULL, 
    [CardTelco] VARCHAR(4) NULL, 
    [CardSerial] VARCHAR(50) NULL, 
    [CardAmount] NUMERIC(16, 2) NULL, 
    [CardTrace] VARCHAR(20) NULL, 
    CONSTRAINT [FK_SaleReceiptDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_SaleReceiptDetail_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]), 
    CONSTRAINT [FK_SaleReceiptDetail_SaleReceipt] FOREIGN KEY ([SaleReceiptId]) REFERENCES [SaleReceipt]([Id]), 
    CONSTRAINT [PK_SaleReceiptDetail] PRIMARY KEY ([SaleReceiptDetailId])
)

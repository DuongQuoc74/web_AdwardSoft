CREATE TABLE [dbo].[PurchaseOrder]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
    [Date] DATE NOT NULL, 
    [SupplierId] INT NOT NULL, 
    [BranchId] INT NOT NULL, 
    [No] CHAR(15) NOT NULL, -- Regex: “PO“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7), Format: example: PO 01 20 12 0000001 → PO0120120000001
    [Description] NVARCHAR(128) NOT NULL, 
    [Status] TINYINT NOT NULL, -- 0 : Hoàn thành đặt hàng , 1 : Đang xử lý, 2 : Đã giao hàng , 3 : Hoàn thành đơn hàng , 4 : Huỷ đơn hàng
    [TotalQuantity] NUMERIC(8, 3) NOT NULL, 
    [SubTotal] NUMERIC(16, 2) NOT NULL, 
    [ShippingFee] NUMERIC(16, 2) NOT NULL, 
    [TaxRate] NUMERIC(5, 2) NOT NULL, 
    [TaxFee] NUMERIC(16, 2) NOT NULL, 
    [TotalDiscount] NUMERIC(16, 2) NOT NULL, 
    [TotalAmount] NUMERIC(16, 2) NOT NULL, 
    [PaymentMethodId] INT NOT NULL, 
    [CreateDate] SMALLDATETIME NOT NULL, 
    [CreatedUser] BIGINT NOT NULL, 
    [ModifiedDate] SMALLDATETIME NOT NULL, 
    [ModifiedUser] BIGINT NOT NULL, 
    [Stockid] INT NOT NULL, 
    [IsPaid] BIT NOT NULL, 
    CONSTRAINT [FK_PurchaseOrder_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [Supplier]([Id]), 
    CONSTRAINT [FK_PurchaseOrder_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id]), 
    CONSTRAINT [FK_PurchaseOrder_PaymentMethod] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethod]([Id])
)

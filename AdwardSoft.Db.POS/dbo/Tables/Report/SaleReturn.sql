CREATE TABLE [dbo].[SaleReturn]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
    [Date] DATE NOT NULL, 
    [SaleReceiptId] CHAR(32) NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [BranchId] INT NOT NULL, 
    [No] CHAR(15) NOT NULL, -- Regex: “SE“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7) Format: example: SE 01 20 12 0000001 → SR0120120000001
    [Description] NVARCHAR(128) NOT NULL, 
    [Status] TINYINT NOT NULL, -- 0 : Phiếu nhập được lập → Default, 1 : Phiếu nhập bị huỷ
    [TotalQuantity] NUMERIC(8, 3) NOT NULL, 
    [SubTotal] NUMERIC(16, 2) NOT NULL, 
    [TaxRate] NUMERIC(5, 2) NOT NULL, 
    [TaxFee] NUMERIC(16, 2) NOT NULL, 
    [TotalDiscount] NUMERIC(16, 2) NOT NULL, 
    [TotalAmount] NUMERIC(16, 2) NOT NULL, 
    [PaymentMethodId] INT NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [ShiftId] INT NOT NULL, 
    [CheckoutCounterId] INT NOT NULL, 
    [CreateDate] SMALLDATETIME NOT NULL, 
    [CreatedUser] BIGINT NOT NULL, 
    [ModifiedDate] SMALLDATETIME NOT NULL, 
    [ModifiedUser] BIGINT NOT NULL, 
    CONSTRAINT [FK_SaleReturn_SaleReceipt] FOREIGN KEY ([SaleReceiptId]) REFERENCES [SaleReceipt]([Id]), 
    CONSTRAINT [FK_SaleReturn_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_SaleReturn_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id]), 
    CONSTRAINT [FK_SaleReturn_PaymentMethod] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethod]([Id]), 
    CONSTRAINT [FK_SaleReturn_Shift] FOREIGN KEY ([ShiftId]) REFERENCES [Shift]([Id]), 
    CONSTRAINT [FK_SaleReturn_CheckoutCounter] FOREIGN KEY ([CheckoutCounterId]) REFERENCES [CheckoutCounter]([Id])
)

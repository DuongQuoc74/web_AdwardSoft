CREATE TABLE [dbo].[SaleReceipt]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
    [Date] SMALLDATETIME NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [BranchId] INT NOT NULL, 
    [No] CHAR(15) NOT NULL, -- Regex: “SR“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7), Format: example: SR 01 20 12 0000001 → SR0120120000001 
    [Description] NVARCHAR(128) NULL, 
    [Status] TINYINT NOT NULL, -- 0 : Hoá đơn được lập, 1 : Hoá đơn bị huỷ
    [IsShipping] BIT NOT NULL DEFAULT 0, 
    [IsCustomerOrder] BIT NOT NULL DEFAULT 0, 
    [TotalQuantity] NUMERIC(13, 3) NOT NULL, 
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
    [EmployeeId] BIGINT NOT NULL, 
    [ShiftId] INT NOT NULL, 
    [CheckoutCounterId] INT NOT NULL, 
    [PriceType] TINYINT NOT NULL, [StockId] INT NOT NULL, 
    [ExchangePoint] NUMERIC(9) NULL, 
    [ExchangeAmount] NUMERIC(16, 2) NULL, 
    [Cash] NUMERIC(16, 2) NULL, 
    [SaleReceiptIdRef] CHAR(32) NULL, 
    --0: Wholesale, 1: Wholesale 2, 2: Retail, 3: Retail 2
    CONSTRAINT [FK_SaleReceipt_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_SaleReceipt_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id]), 
    CONSTRAINT [FK_SaleReceipt_PaymentMethod] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethod]([Id]), 
    CONSTRAINT [FK_SaleReceipt_Shift] FOREIGN KEY ([ShiftId]) REFERENCES [Shift]([Id]), 
    CONSTRAINT [FK_SaleReceipt_CheckoutCounter] FOREIGN KEY ([CheckoutCounterId]) REFERENCES [CheckoutCounter]([Id]), 
    CONSTRAINT [FK_SaleReceipt_Stock] FOREIGN KEY ([StockId]) REFERENCES [Stock]([Id])
)

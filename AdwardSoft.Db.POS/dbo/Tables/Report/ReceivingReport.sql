CREATE TABLE [dbo].[ReceivingReport]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
    [Date] DATE NOT NULL, 
    [SupplierId] INT NOT NULL, 
    [BranchId] INT NOT NULL, 
    [No] CHAR(15) NOT NULL, -- not null unique Regex: “RR“+ Quầy (2) + Năm(2) + Tháng(2) + Idx (7) Format: example: SR 01 20 12 0000001 → RR0120120000001
    [Description] NVARCHAR(128) NOT NULL, 
    [Status] TINYINT NOT NULL, -- 0: Wating - Chờ duyệt, 1: Approved - Đã duyệt, 2 : Trash - Xoá
    [IsPurchaseOrder] BIT NOT NULL DEFAULT 0, 
    [TotalQuantity] NUMERIC(8, 3) NOT NULL, 
    [SubTotal] NUMERIC(16, 2) NOT NULL, 
    [TaxRate] NUMERIC(5, 2) NOT NULL, 
    [TaxFee] NUMERIC(16, 2) NOT NULL, 
    [TotalDiscount] NUMERIC(16, 2) NOT NULL, 
    [TotalAmount] NUMERIC(16, 2) NOT NULL, 
    [PaymentMethodId] INT NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [CreateDate] SMALLDATETIME NOT NULL, 
    [CreatedUser] BIGINT NOT NULL, 
    [ModifiedDate] SMALLDATETIME NOT NULL, 
    [ModifiedUser] BIGINT NOT NULL, 
    CONSTRAINT [FK_ReceivingReport_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [Supplier]([Id]), 
    CONSTRAINT [FK_ReceivingReport_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id])
)

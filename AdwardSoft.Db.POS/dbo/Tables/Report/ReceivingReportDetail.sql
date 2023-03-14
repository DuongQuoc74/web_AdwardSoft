CREATE TABLE [dbo].[ReceivingReportDetail]
(
	[ReceivingReportDetailId] CHAR(32) NOT NULL PRIMARY KEY, 
    [ReceivingReportId] CHAR(32) NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] NUMERIC(8, 3) NOT NULL, 
    [UnitId] INT NOT NULL, 
    [Price] NUMERIC(16, 2) NOT NULL, 
    [Discount] NUMERIC(16, 2) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [IsPromo] BIT NOT NULL, 
    CONSTRAINT [FK_ReceivingReportDetail_ReceivingReport] FOREIGN KEY ([ReceivingReportId]) REFERENCES [ReceivingReport]([Id]), 
    CONSTRAINT [FK_ReceivingReportDetail_Unit] FOREIGN KEY ([UnitId]) REFERENCES [Unit]([Id]), 
    CONSTRAINT [FK_ReceivingReportDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)

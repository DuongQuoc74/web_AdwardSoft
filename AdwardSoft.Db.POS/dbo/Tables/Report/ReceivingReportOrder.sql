CREATE TABLE [dbo].[ReceivingReportOrder]
(
	[ReceivingReportId] CHAR(32) NOT NULL , 
    [PurchaseOrderId] CHAR(32) NOT NULL, 
    PRIMARY KEY ([ReceivingReportId], [PurchaseOrderId]), 
    CONSTRAINT [FK_ReceivingReportOrder_ReceivingReport] FOREIGN KEY ([ReceivingReportId]) REFERENCES [ReceivingReport]([Id]), 
    CONSTRAINT [FK_ReceivingReportOrder_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrder]([Id])
)

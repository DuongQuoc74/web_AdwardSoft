CREATE TABLE [dbo].[SaleReceiptPromotion]
(
	[SaleReceiptDetailId] CHAR(32) NOT NULL , 
    [PromotionId] INT NOT NULL, 
    [SaleReceiptId] CHAR(32) NOT NULL, 
    PRIMARY KEY ([SaleReceiptId], [SaleReceiptDetailId], [PromotionId])
)

CREATE TABLE [dbo].[PromotionDirect]
(
	[PromotionId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    [PromoType] TINYINT NOT NULL, -- 0 : By value (Giảm giá trị), 1 : By rate (Giảm tỷ lệ)
    [PromoAmount] NUMERIC(16, 2) NULL, 
    [PromoRate] NUMERIC(5, 3) NULL, 
    PRIMARY KEY ([PromotionId], [ProductId]), 
    CONSTRAINT [FK_PromotionDirect_Promotion] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion]([Id]), 
    CONSTRAINT [FK_PromotionDirect_PurchaseProduct] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)

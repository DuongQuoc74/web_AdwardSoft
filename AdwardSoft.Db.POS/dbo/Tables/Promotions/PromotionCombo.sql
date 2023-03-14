CREATE TABLE [dbo].[PromotionCombo]
(
	[PromotionId] INT NOT NULL , 
    [PurchaseProductId] INT NOT NULL, 
    [PromoProductId] INT NOT NULL, 
    [PromoType] TINYINT NOT NULL,  -- 0 : By value (Giảm giá trị), 1 : By rate (Giảm tỷ lệ)
    [PromoAmount] NUMERIC(16, 2) NOT NULL, 
    PRIMARY KEY ([PromotionId], [PurchaseProductId], [PromoProductId]), 
    CONSTRAINT [FK_PromotionCombo_Promotion] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion]([Id]), 
    CONSTRAINT [FK_PromotionCombo_PurchaseProduct] FOREIGN KEY ([PurchaseProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_PromotionCombo_PromoProduct] FOREIGN KEY ([PromoProductId]) REFERENCES [Product]([Id])
)

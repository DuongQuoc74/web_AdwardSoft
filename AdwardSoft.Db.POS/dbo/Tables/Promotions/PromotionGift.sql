CREATE TABLE [dbo].[PromotionGift]
(
	[PromotionId] INT NOT NULL , 
    [PurchaseProductId] INT NOT NULL, 
    [PurchaseQuantity] NUMERIC(13, 3) NOT NULL, 
    [GiftProductId] INT NOT NULL, 
    [GiftQuantity] NUMERIC(13, 3) NOT NULL, 
    CONSTRAINT [FK_PromotionGift_Promotion] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion]([Id]), 
    CONSTRAINT [FK_PromotionGift_PurchaseProduct] FOREIGN KEY ([PurchaseProductId]) REFERENCES [Product]([Id]),  
    CONSTRAINT [FK_PromotionGift_GiftProduct] FOREIGN KEY ([GiftProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [PK_PromotionGift] PRIMARY KEY ([PromotionId], [PurchaseProductId], [GiftProductId])
)

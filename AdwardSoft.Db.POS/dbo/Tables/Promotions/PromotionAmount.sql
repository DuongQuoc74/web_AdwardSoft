CREATE TABLE [dbo].[PromotionAmount]
(
	[PromotionId] INT NOT NULL , 
    [Idx] CHAR(13) NOT NULL, --> Guid
    [PurchaseType] TINYINT, -- 0: Theo khối lượng, 1: Theo giá trị 
    [ProductId] INT, --Chỉ áp dụng chọn khi PurchaseType = 0, nếu không chọn là tất cả sản phẩm
    [QtyFrom] NUMERIC(13, 3) NOT NULL,
    [QtyTo] NUMERIC(13, 3) NOT NULL,
    [AmountFrom] NUMERIC(16, 2) NOT NULL, 
    [AmountTo] NUMERIC(16, 2) NOT NULL, 
    [PromoType] TINYINT NOT NULL, -- 0 : By value (Giảm giá trị), 1 : By rate (Giảm tỷ lệ)
    [PromoAmount] NUMERIC(16, 2) NOT NULL, 
    [PromoRate] NUMERIC(5, 3) NOT NULL, 
    PRIMARY KEY ([PromotionId], [Idx]), 
    CONSTRAINT [FK_PromotionAmount_Promotion] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion]([Id])
)

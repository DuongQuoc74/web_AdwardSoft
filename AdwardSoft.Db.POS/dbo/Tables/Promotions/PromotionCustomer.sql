CREATE TABLE [dbo].[PromotionCustomer]
(
	[PromotionId] INT NOT NULL , 
    [CustomerId] INT NOT NULL, 
    PRIMARY KEY ([PromotionId], [CustomerId])
)

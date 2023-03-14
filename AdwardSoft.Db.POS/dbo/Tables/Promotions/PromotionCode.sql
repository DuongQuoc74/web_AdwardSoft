CREATE TABLE [dbo].[PromotionCode]
(
	[PromotionId] INT NOT NULL , 
    [Code] CHAR(20) NOT NULL, 
    PRIMARY KEY ([Code], [PromotionId])
)

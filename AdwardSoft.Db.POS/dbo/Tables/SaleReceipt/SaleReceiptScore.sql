CREATE TABLE [dbo].[SaleReceiptScore]
(
	[SaleReceiptId] CHAR(32) NOT NULL PRIMARY KEY, 
    [ScoreConfigurationId] INT NOT NULL, 
    [Point] NUMERIC(9) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    CONSTRAINT [FK_SaleReceiptScore_SaleReceipt] FOREIGN KEY ([SaleReceiptId]) REFERENCES [SaleReceipt]([Id]), 
    CONSTRAINT [FK_SaleReceiptScore_ScoreConfiguration] FOREIGN KEY ([ScoreConfigurationId]) REFERENCES [ScoreConfiguration]([Id])
)

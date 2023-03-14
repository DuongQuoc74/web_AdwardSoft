CREATE TABLE [dbo].[CustomerOrderScore]
(
	[OrderId] CHAR(32) NOT NULL PRIMARY KEY, 
    [ScoreConfigurationId] INT NOT NULL, 
    [Point] NUMERIC(9) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    CONSTRAINT [FK_CustomerOrderScore_CustomerOrder] FOREIGN KEY ([OrderId]) REFERENCES [CustomerOrder]([Id])
)

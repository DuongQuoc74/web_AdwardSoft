CREATE TABLE [dbo].[MembershipScore]
(
	[CustomerId] INT NOT NULL , 
    [SaleReceiptId] CHAR(32) NOT NULL, 
    [Type] TINYINT NOT NULL, -- 0:Exchange , 1:Earn, 2: Return
    [Year] CHAR(4) NOT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [Point] NUMERIC(9) NOT NULL, 
    PRIMARY KEY ([Type], [CustomerId], [SaleReceiptId]), 
    CONSTRAINT [FK_MembershipScore_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id])
)

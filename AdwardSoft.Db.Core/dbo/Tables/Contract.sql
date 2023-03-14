CREATE TABLE [dbo].[Contract]
(
	[OrderId] CHAR(32) NOT NULL PRIMARY KEY, 
    [Date] SMALLDATETIME NOT NULL, 
    [SaleId] BIGINT NOT NULL, 
    [Index] INT NOT NULL, 
    [No] CHAR(9) NOT NULL, 
    [Type] TINYINT NOT NULL, 
    [IsConfirm] BIT NOT NULL DEFAULT 0
)

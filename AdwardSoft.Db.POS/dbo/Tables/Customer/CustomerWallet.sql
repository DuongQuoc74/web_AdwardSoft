CREATE TABLE [dbo].[CustomerWallet]
(
	[Id] CHAR(13) NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Note] NVARCHAR(300) NOT NULL, 
    [BankAccountId] INT NULL, 
    [Amount] NUMERIC(16, 2) NOT NULL, 
    [Status] TINYINT NOT NULL, --0: Đang xử lý, 1: Thực hiện thành công, 2: Từ chối
    [CustomerId] INT NOT NULL, 
    [Type] TINYINT NOT NULL -- 0: Tiền nạp, 1: Tiền thanh toán
, 
    CONSTRAINT [PK_CustomerWallet] PRIMARY KEY ([Id])    )

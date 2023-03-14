CREATE TABLE [dbo].[CheckoutCounter]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(120) NOT NULL, 
    [Status] TINYINT NOT NULL, -- 0 Unavailable (Ngừng hoạt động) , 1 Available (Đang hoạt động)
    [BranchId] INT NOT NULL, 
    CONSTRAINT [FK_CheckoutCounter_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id])
)

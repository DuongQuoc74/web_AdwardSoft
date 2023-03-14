CREATE TABLE [dbo].[IPAccess]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Type] TINYINT NOT NULL, --  0: Theo khoảng, 1: Theo địa chỉ xác định
    [Filter] TINYINT NOT NULL, -- 0: Whitelist, 1: Backlist
    [IP1] VARCHAR(15) NOT NULL, 
    [IP2] VARCHAR(15) NULL
)

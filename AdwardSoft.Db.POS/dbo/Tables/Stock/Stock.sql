CREATE TABLE [dbo].[Stock]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(60) NOT NULL, 
    [BranchId] INT NOT NULL, 
    [Description] NVARCHAR(100) NULL, 
    -- 0. Inventory Tracking (Theo dõi tồn kho)
    -- 1. No Inventory Tracking (Không theo dõi tồn kho)
    [Type] TINYINT NOT NULL, 
    [IsDefault] BIT NOT NULL

    CONSTRAINT [FK_Stock_Branch] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([Id]), 
)

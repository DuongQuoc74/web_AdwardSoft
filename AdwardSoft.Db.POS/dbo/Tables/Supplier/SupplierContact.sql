CREATE TABLE [dbo].[SupplierContact]
(
	[SupplierId] INT NOT NULL ,
	[Idx] INT NOT NULL ,
    [Phone] CHAR(10) NOT NULL, 
    [Name] NVARCHAR(32) NOT NULL, 
    [Position] NVARCHAR(50) NOT NULL, 
    [Note] NVARCHAR(100) NULL, 
    [IsDefault] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_SupplierContact_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [Supplier]([Id]), 
    CONSTRAINT [PK_SupplierContact] PRIMARY KEY ([SupplierId], [Idx])
)

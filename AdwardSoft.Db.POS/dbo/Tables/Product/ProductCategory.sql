CREATE TABLE [dbo].[ProductCategory]
(
	[ProductId] INT NOT NULL , 
    [CategoryId] INT NOT NULL, 
    [IsDefault] BIT NOT NULL, 
    PRIMARY KEY ([ProductId], [CategoryId]), 
    CONSTRAINT [FK_ProductCategory_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_ProductCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
)

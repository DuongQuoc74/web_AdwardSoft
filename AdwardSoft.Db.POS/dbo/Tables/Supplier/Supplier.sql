CREATE TABLE [dbo].[Supplier]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(120) NOT NULL, 
    [Address] NVARCHAR(128) NOT NULL, 
    [Tel] VARCHAR(12) NULL, 
    [Email] VARCHAR(254) NULL, 
    [IsDefault] BIT NULL
)

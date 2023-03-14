CREATE TABLE [dbo].[ServiceDisplay]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(120) NOT NULL, 
    [SupplierId] INT NOT NULL, 
    [Date] DATE NOT NULL, 
    [DateFrom] DATE NOT NULL, 
    [DateTo] DATE NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    [Fee] NUMERIC(16, 2) NOT NULL, 
    [PaymentPeriod] TINYINT NOT NULL, -- 0: Month, 1: Quater, 2: Year
    CONSTRAINT [FK_ServiceDisplay_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [Supplier]([Id])
)

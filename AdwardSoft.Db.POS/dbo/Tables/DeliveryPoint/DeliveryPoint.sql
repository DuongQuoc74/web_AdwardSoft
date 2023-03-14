CREATE TABLE [dbo].[DeliveryPoint]
(
	[Id] INT IDENTITY(1,1) not null PRIMARY KEY,
	[Code] VARCHAR(6) UNIQUE not null,
	[Name] NVARCHAR(120) NOT NULL,
	[Rate] NUMERIC(3) DEFAULT 0,
	[LocationId] INT,
	[Status] TINYINT DEFAULT 1,
	CONSTRAINT [FK_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Location]
	)




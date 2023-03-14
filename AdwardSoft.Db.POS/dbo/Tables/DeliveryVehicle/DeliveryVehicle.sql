CREATE TABLE [dbo].[DeliveryVehicle]
(
	[Id] INT IDENTITY (1,1) PRIMARY KEY,
	[LicensePlate] VARCHAR (15) UNIQUE NOT NULL,
	[Name] NVARCHAR (120) NOT NULL,
	[DriverName] NVARCHAR(32) NOT NULL,
	[DriverPhone] VARCHAR(10) NOT NULL,
	[Load] NUMERIC(4,3) NOT NULL ,
	[VehicleTypeId] INT NOT NULL,
						--CONSTRAINT [FK_DeliveryVehicle_VehicleTypeId] FOREIGN KEY ([VehicleTypeId]) REFERENCES [Location]([Id])
	[CustomerId] INT NOT NULL,
						--CONSTRAINT [FK_DeliveryVehicle_CustomerId]foreign key ([CustomerId]) REFERENCES Customer(Id)
	[Status] TINYINT DEFAULT 1
	--1: Available - Khả dụng
    --0: Unavailable - Không khả dụng
)

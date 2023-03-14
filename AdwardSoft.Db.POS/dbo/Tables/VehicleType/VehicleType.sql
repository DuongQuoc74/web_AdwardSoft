create table [dbo].[VehicleType]
(
	[Id] int identity (1,1) primary key,
	[Name] nvarchar(120) not null,
	[Status] tinyint default 1
	--1: Available - Khả dụng
	--0: Unavailable - Không khả dụng

)
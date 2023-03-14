CREATE TABLE [dbo].[Setting]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ProjectName] NVARCHAR(120) NULL, 
    [CompanyName] NVARCHAR(120) NULL, 
    [Address] NVARCHAR(120) NULL, 
    [Tel] VARCHAR(10) NULL, 
    [Website] VARCHAR(120) NULL, 
    [MapCoordinateLat] NUMERIC(10, 7) NULL, 
    [MapCoordinateLong] NUMERIC(10, 7) NULL, 
    [ErrorRange] NUMERIC(3) NULL
)

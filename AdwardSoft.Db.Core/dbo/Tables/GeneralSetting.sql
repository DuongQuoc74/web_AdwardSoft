CREATE TABLE [dbo].[GeneralSetting]
(
	[Id] TINYINT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(71) NOT NULL, 
    [Description] NVARCHAR(160) NOT NULL, 
    [TagLine] NVARCHAR(71) NOT NULL, 
    [InsideURL] VARCHAR(2048) NOT NULL, 
    [SiteURL] VARCHAR(2048) NOT NULL, 
    [EmailAddress] VARCHAR(150) NOT NULL, 
    [IsRegisterMembership] BIT NOT NULL, 
    [DefaultRoleId] INT NOT NULL, 
    [DefaultLaguageCode] CHAR(2) NOT NULL, 
    [DateFormat] VARCHAR(20) NOT NULL, 
    [TimeZone] VARCHAR(10) NOT NULL, 
    [Permalink] VARCHAR(2048) NOT NULL, 
)

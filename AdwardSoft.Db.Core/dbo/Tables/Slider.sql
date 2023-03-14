CREATE TABLE [dbo].[Slider]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(128) NOT NULL, 
    [Source] VARCHAR(2048) NOT NULL, 
    [SourceType] TINYINT NOT NULL, -- 0: Image, 1: Video
    [Sort] TINYINT NULL, 
    [RedirectUrl] VARCHAR(2048) NOT NULL, 
    [RedirectType] TINYINT NOT NULL -- 0: blank, 1: _self, 2: _parent, 3: _top
)
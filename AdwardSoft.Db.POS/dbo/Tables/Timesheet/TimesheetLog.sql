CREATE TABLE [dbo].[TimesheetLog]
(
	[Id] CHAR(32) NOT NULL PRIMARY KEY, 
	[UserId] BIGINT NOT NULL, 
    [Date] SMALLDATETIME NOT NULL, 
    [Type] TINYINT NOT NULL, -- 0 : IN , 1 : OUT
    [MapCoordinateLong] NUMERIC(10, 7) NOT NULL, 
    [MapCoordinateLat] NUMERIC(10, 7) NOT NULL
)

CREATE TABLE [dbo].[TimeSheet]
(
	[UserId] BIGINT NOT NULL , 
    [Date] DATE NOT NULL, 
    [Type] TINYINT NOT NULL, 
    [Reason] NVARCHAR(150) NULL, 
    [StartTime] VARCHAR(5) NULL, 
    [EndTime] VARCHAR(5) NULL, 
    [CreatedDate] SMALLDATETIME NOT NULL, 
    PRIMARY KEY ([Date], [UserId])
)

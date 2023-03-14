CREATE TABLE [dbo].[UserInfoHistory]
(
	[UserId] BIGINT NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Status] TINYINT NOT NULL, -- -1.Rejected, 0.Pending, 1.Approved
    [FullName] NVARCHAR(128) NULL, 
    [DoB] DATE NULL

    PRIMARY KEY([UserId], [Date])
)

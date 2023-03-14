CREATE TABLE [dbo].[UserBranch]
(
	[UserId] BIGINT NOT NULL , 
    [BranchId] INT NOT NULL, 
    PRIMARY KEY ([UserId], [BranchId])
)

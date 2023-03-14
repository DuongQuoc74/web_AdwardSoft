CREATE TABLE [dbo].[CustomerClass]
(
	[CustomerId] INT NOT NULL, 
    [MembershipClassId] INT NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [OldMembershipClassId] INT NOT NULL,

    PRIMARY KEY([CustomerId], [MembershipClassId]),

    CONSTRAINT [FK_CustomerClass_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]), 
    CONSTRAINT [FK_CustomerClass_MembershipClass] FOREIGN KEY ([MembershipClassId]) REFERENCES [MembershipClass]([Id]),
)

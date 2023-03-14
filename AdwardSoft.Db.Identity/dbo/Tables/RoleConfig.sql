CREATE TABLE [dbo].[RoleConfig]
(
	[RoleId] INT NOT NULL PRIMARY KEY, 
    [PwdRegex] VARCHAR(150) NOT NULL, 
    [VerificationMethod] TINYINT NOT NULL, -- 0: SMS, 1: Email 
    [IsLogging] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_RoleConfig_Role FOREIGN KEY (RoleId) REFERENCES [Role](Id)
)
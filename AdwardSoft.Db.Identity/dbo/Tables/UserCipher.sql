CREATE TABLE [dbo].[UserCipher]
(
	[UserId] BIGINT NOT NULL PRIMARY KEY, 
    [AccessKey] VARCHAR(32) NOT NULL, 
    [PublicKey] VARCHAR(4000) NOT NULL, 
    [SecretKey] VARCHAR(32) NOT NULL, 
    [PartnerCode] VARCHAR(32) NOT NULL 
)

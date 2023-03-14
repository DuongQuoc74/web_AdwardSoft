/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[Role])
BEGIN
	INSERT INTO [dbo].[Role] VALUES('Administrator','Admin', '', 0, 1,0)
	INSERT INTO [dbo].[Role] VALUES('Manager','Manager', '', 0, 2,0)
	INSERT INTO [dbo].[Role] VALUES('Editor','Editor', '', 0, 3,0)
END
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[User])
BEGIN
INSERT [dbo].[User] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [FullName], [Avatar], [IdentityNumber]) 
VALUES (1, N'admin@adwardsoft.com', N'ADMIN@ADWARDSOFT.COM', N'admin@adwardsoft.com', N'ADMIN@ADWARDSOFT.COM', 0, N'AQAAAAEAACcQAAAAEFBCo8KQXTV7oBu4T6cBsNFr4PmOCWmEU8Xv8q0EF1NrAYtWCZIwZGVNVIz4hNzVeg==', N'PLS66W5FHPMKUBOHSFHBGQT7IK7F7MKU', NULL, '987654321', 0, 0, NULL, 1, 0, N'AdwardSoft', NULL, '123456789')

INSERT INTO [dbo].[UserRole] VALUES(1,1)
END
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
CREATE PROCEDURE [dbo].[usp_User_ReadUserIsNotReferenceToMerchant]
@Type INT,
@Status INT
AS
	SELECT U.[Id], U.[FullName] FROM [dbo].[User] AS U
	WHERE U.[Id] NOT IN (SELECT [UserId] FROM [AdsGS.GS].[dbo].[Merchant]) 
	AND U.[Type] = @Type AND U.[Status] = @Status
RETURN 0

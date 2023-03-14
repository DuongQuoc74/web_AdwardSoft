CREATE PROCEDURE [dbo].[usp_Select_ReadDeliveryPointByLocationId]
	@LocationId INT
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[DeliveryPoint]
	WHERE [Status] = 1 AND [LocationId] = @LocationId

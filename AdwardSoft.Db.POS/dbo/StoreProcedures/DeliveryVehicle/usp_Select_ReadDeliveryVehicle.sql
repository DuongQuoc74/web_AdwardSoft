CREATE PROCEDURE [dbo].[usp_Select_ReadDeliveryVehicle]
	@CustomerId INT
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[DeliveryVehicle]
	WHERE [CustomerId] = @CustomerId AND [Status] = 1

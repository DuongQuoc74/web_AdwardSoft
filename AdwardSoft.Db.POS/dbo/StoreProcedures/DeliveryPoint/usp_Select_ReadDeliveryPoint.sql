CREATE PROCEDURE [dbo].[usp_Select_ReadDeliveryPoint]
(@Status TINYINT = 1)
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[DeliveryPoint]
	WHERE [Status] = @Status

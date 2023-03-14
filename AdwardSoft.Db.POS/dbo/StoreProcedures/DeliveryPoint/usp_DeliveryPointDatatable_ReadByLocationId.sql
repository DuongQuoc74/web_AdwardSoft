CREATE PROCEDURE [dbo].[usp_DeliveryPointDatatable_ReadByLocationId]
	@LocationId INT
AS
BEGIN

	SELECT * 
	FROM [dbo].[DeliveryPoint]
	WHERE [LocationId] = @LocationId

END

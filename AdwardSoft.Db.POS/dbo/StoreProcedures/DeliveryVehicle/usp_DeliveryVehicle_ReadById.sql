CREATE PROCEDURE [dbo].[usp_DeliveryVehicle_ReadById]
	@Id INT
AS
	SELECT * 
	FROM[dbo].[DeliveryVehicle]
	WHERE [Id] = @Id
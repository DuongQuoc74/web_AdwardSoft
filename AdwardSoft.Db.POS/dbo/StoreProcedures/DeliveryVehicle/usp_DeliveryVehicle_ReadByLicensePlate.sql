CREATE PROCEDURE [dbo].[usp_DeliveryVehicle_ReadByLicensePlate]
	@Id INT,
	@LicensePlate VARCHAR(15)
AS 
SELECT *
FROM [dbo].[DeliveryVehicle]
WHERE [LicensePlate] = @LicensePlate AND [Id] <> @Id
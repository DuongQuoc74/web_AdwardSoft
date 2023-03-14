CREATE PROCEDURE [dbo].[usp_DeliveryVehicle_Update]
	@Id	INT,
	@LicensePlate VARCHAR (15),
	@Name NVARCHAR(120),
	@DriverName NVARCHAR(32),
	@DriverPhone VARCHAR(10),
	@Load NUMERIC(4,3),
	@VehicleTypeId INT,
	@CustomerId INT,
	@Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	 BEGIN TRY
        BEGIN TRAN
			UPDATE [dbo].[DeliveryVehicle]
			SET [LicensePlate] = @LicensePlate,
				[Name] = @Name,
				[DriverName] = @DriverName,
				[DriverPhone] = @DriverPhone,
				[Load] = @Load,
				[VehicleTypeId] = @VehicleTypeId,
				[CustomerId] = @CustomerId,
				[Status] = @Status
			WHERE [Id] = @Id
         COMMIT
         SELECT DV.*
		 FROM [dbo].[DeliveryVehicle] AS DV
		 WHERE DV.[Id]=@Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
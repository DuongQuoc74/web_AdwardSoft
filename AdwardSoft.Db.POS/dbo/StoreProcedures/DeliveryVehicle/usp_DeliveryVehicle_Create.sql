CREATE PROCEDURE [dbo].[usp_DeliveryVehicle_Create]
	@Id	INT, 
	@LicensePlate VARCHAR (15), 
	@Name NVARCHAR(120), 
	@DriverName NVARCHAR(32), 
	@DriverPhone VARCHAR(10), 
	@Load NUMERIC(4, 3), 
	@VehicleTypeId INT, 
	@CustomerId INT, 
	@Status TINYINT
AS
BEGIN
	--SET IDENTITY_INSERT [DeliveryVehicle] ON
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	 BEGIN TRY
        BEGIN TRAN
			INSERT INTO [dbo].[DeliveryVehicle] 
			([LicensePlate], [Name], [DriverName], [DriverPhone], [Load], [VehicleTypeId], [CustomerId], [Status]) 
			VALUES 
			(@LicensePlate, @Name, @DriverName, @DriverPhone, @Load, @VehicleTypeId, @CustomerId, @Status)
			SET @Id = SCOPE_IDENTITY()
			COMMIT
			SELECT DV.*
			FROM [dbo].[DeliveryVehicle] AS DV
			WHERE DV.[Id] = @Id
		END TRY
		BEGIN CATCH
			ROLLBACK;
			SELECT 0;
			THROW;
		END CATCH
END		

CREATE PROCEDURE [dbo].[usp_DeliveryVehicleDatatable_Read]
	@LicensePlate VARCHAR(15),
	@CustomerId INT = 0,
	@VehicleTypeId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@Param NVARCHAR(2000) = ' @LicensePlate VARCHAR(15),
										  @CustomerId INT,
										  @VehicleTypeId INT '
		
		SET @SqlStr = N'SELECT D.*, C.[Name] AS [CustomerName], V.[Name] AS [VehicleTypeName]
						FROM [dbo].[DeliveryVehicle] AS D 
						INNER JOIN [dbo].[Customer] AS C ON D.[CustomerId] = C.[Id]
						INNER JOIN [dbo].[VehicleType] AS V ON D.[VehicleTypeId] = V.[Id]
						WHERE 1 = 1 '

		BEGIN

			IF @CustomerId <> 0
				SET @SqlStr = @SqlStr + N' AND D.[CustomerId] = @CustomerId '

			IF @VehicleTypeId <> 0
				SET @SqlStr = @SqlStr + N' AND D.[VehicleTypeId] = @VehicleTypeId '

			IF(@LicensePlate <> 'NULL')
				SET @SqlStr =  @SqlStr + N' AND D.[LicensePlate] LIKE ''%'' + @LicensePlate + ''%'' '

			SET @SqlStr = @SqlStr + N' ORDER BY D.[Name] ASC'

			EXEC SP_EXECUTESQL @SqlStr, 
						   @Param,
						   @LicensePlate,
						   @CustomerId,
						   @VehicleTypeId
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

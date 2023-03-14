CREATE PROCEDURE [dbo].[usp_Select_ReadVehicleType]
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[VehicleType]
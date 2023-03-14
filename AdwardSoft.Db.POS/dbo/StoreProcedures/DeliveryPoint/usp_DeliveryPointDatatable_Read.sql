CREATE PROCEDURE [dbo].[usp_DeliveryPointDatatable_Read]
AS
	SELECT D.*, L.Name AS [LocationName]
	FROM [dbo].[DeliveryPoint] AS D INNER JOIN [dbo].[Location] AS L ON D.LocationId = L.[Id]
	

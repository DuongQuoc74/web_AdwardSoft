CREATE PROCEDURE [dbo].[usp_PriceDetail_ReadById]
	@ProductId INT,
	@LocationId INT,
	@DeliveryPointId INT,
	@DeliveryType TINYINT,
	@Date DATE
AS
	SELECT PD.*
	FROM [dbo].[PriceDetail] AS PD INNER JOIN [dbo].[Product] AS P ON PD.[ProductId] = P.[Id]
								   INNER JOIN [dbo].[Location] AS L ON PD.[LocationId] = L.[Id]
								   INNER JOIN [dbo].[DeliveryPoint] AS D ON PD.[DeliveryPointId] = D.[Id]
								   INNER JOIN [dbo].[PriceList] AS PL ON PD.[Date] = PL.[Date]
								   
	WHERE PD.[ProductId] = @ProductId AND PD.[DeliveryPointId] = @DeliveryPointId 
			AND PD.[LocationId] = @LocationId AND PD.[DeliveryType] = @DeliveryType AND PD.[Date] = @Date

CREATE PROCEDURE [dbo].[usp_CustomerOrderDetailDatatable_ReadProductList]
AS
BEGIN
	DECLARE @Amount INT = (SELECT COUNT(DISTINCT [ProductId]) FROM [dbo].[PriceDetail])

	SELECT TOP (@Amount) P.[Id] AS [ProductId], P.[Code] AS [ProductCode], P.[Name] AS [ProductName], U.[Id] AS [UnitId], U.[Name] AS [UnitName], PD.[Price], PD.[Date]
	FROM [dbo].[PriceDetail] AS PD INNER JOIN  [dbo].[Product] AS P ON P.[Id] = PD.[ProductId]
							       INNER JOIN [dbo].[Unit] AS U ON P.[UnitId] = U.[Id]
	ORDER BY PD.[Date] DESC
END

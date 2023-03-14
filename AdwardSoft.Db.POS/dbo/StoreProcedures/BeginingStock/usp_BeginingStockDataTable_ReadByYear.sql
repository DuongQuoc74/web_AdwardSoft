CREATE PROCEDURE [dbo].[usp_BeginingStockDataTable_ReadByYear]
	@StockId INT,
	@ProductId INT,
	@Year CHAR(4),
	@UnitId INT
AS
	SELECT BT.*, P.[Name] AS [ProductName], P.[Code] AS [ProductCode], 
	S.[Name] AS [StockName],  U.[Name] AS [UnitName], BT.[Quantity], COUNT(*) OVER() AS [Count]
	FROM [dbo].[BeginingStock] AS BT
	INNER JOIN [dbo].[Product] AS P ON BT.[ProductId] = P.[Id]
	INNER JOIN [dbo].[Stock] AS S ON BT.[StockId] = S.[Id]
	INNER JOIN [dbo].[Unit] AS U ON BT.[UnitId] = U.[Id]
	WHERE BT.[StockId] = @StockId 
	AND [ProductId] = @ProductId
	AND BT.[Year] = @Year
	AND BT.[UnitId] = @UnitId

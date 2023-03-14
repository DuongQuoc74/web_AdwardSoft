CREATE PROCEDURE [dbo].[usp_StockTakingDatatable_ReadByDate]
	@ProductId INT,
	@StockId INT,
	@Date DATE,
	@UnitId  INT
AS
	SELECT PT.*, U.[Name]
	FROM [dbo].[Stocktaking] PT
    INNER JOIN [dbo].[Unit] AS U ON PT.[UnitId] = U.[Id]
	WHERE PT.[StockId] = @StockId
	AND PT.[ProductId] = @ProductId
	AND PT.[Date] = (SELECT CONVERT(DATE, @Date))
	

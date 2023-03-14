CREATE PROCEDURE [dbo].[usp_StockTaking_Read]
AS
	SELECT PT.*, U.[Name]
	FROM [dbo].[Stocktaking] PT
    INNER JOIN [dbo].[Unit] AS U ON PT.[UnitId] = U.[Id]

CREATE PROCEDURE [dbo].[usp_Stock_ReadStockByDate]
	@StartDate DATE,
	@EndDate DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[Stocktaking] ST,[dbo].[Stock] S
		WHERE ST.[StockId]=S.[Id] AND [Date]<=@StartDate AND [Date]>=@EndDate
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


CREATE PROCEDURE [dbo].[usp_SellingPrice_ReadByProduct]
	@ProductId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT 
			MAX(WholesalePrice) AS [WholesalePrice],
			MAX(RetailPrice) AS [RetailPrice],
			MAX([Date]) AS [Date],
			ProductId, UnitId
		FROM [SellingPrice] 
		WHERE [ProductId] = @ProductId AND [Date] <= GETDATE()
		AND [Date] IN (SELECT MAX([Date]) FROM [SellingPrice] 
		WHERE [ProductId] = @ProductId AND [Date] <= GETDATE() GROUP BY [ProductId], [UnitId])
		GROUP BY [ProductId], [UnitId]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

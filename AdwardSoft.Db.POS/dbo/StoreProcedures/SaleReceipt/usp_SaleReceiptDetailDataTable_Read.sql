CREATE PROCEDURE [dbo].[usp_SaleReceiptDetailDataTable_Read]
	@SaleReceiptId CHAR(32)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT srd.*, p.[Name] AS [ProductName], 
		pm.[Name] AS [PromotionName], p.[Code] AS [ProductCode]
		FROM [dbo].[SaleReceiptDetail] srd
		INNER JOIN [dbo].[Product] p ON srd.[ProductId] = p.[Id]
		LEFT JOIN [dbo].[SaleReceiptPromotion] sp ON srd.[SaleReceiptDetailId] = sp.[SaleReceiptDetailId]
		LEFT JOIN [dbo].[Promotion] pm ON sp.[PromotionId] = pm.[Id]
		WHERE srd.[SaleReceiptId] = @SaleReceiptId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

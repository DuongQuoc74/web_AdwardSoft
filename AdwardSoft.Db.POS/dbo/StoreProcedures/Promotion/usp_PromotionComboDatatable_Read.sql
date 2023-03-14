CREATE PROCEDURE [dbo].[usp_PromotionComboDatatable_Read]
	@PromotionId int 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* , ppro.[Name] AS [PromoProductName], ppur.[Name] AS [PurchaseProductName]
		FROM [dbo].[PromotionCombo] p
		INNER JOIN [Product] ppro ON ppro.[Id] = p.[PromoProductId]
		INNER JOIN [Product] ppur ON ppur.[Id] = p.[PurchaseProductId]
		WHERE p.[PromotionId] = @PromotionId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
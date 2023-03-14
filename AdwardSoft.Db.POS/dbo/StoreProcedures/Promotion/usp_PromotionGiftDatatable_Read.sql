CREATE PROCEDURE [dbo].[usp_PromotionGiftDatatable_Read]
	@PromotionId INT 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.*, pgif.[Name] AS [GiftProductName], ppur.[Name] AS [PurchaseProductName]
		FROM [dbo].[PromotionGift] p
		INNER JOIN [Product] pgif ON pgif.[Id] = p.[GiftProductId]
		INNER JOIN [Product] ppur ON ppur.[Id] = p.[PurchaseProductId]
		WHERE p.[PromotionId] = @PromotionId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
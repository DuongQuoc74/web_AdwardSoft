CREATE PROCEDURE [dbo].[usp_PromotionGift_ReadById]
	@PromotionId INT ,
	@GiftProductId INT,
	@PurchaseProductId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[PromotionGift] p
		WHERE p.[PromotionId] = @PromotionId 
		AND p.[GiftProductId] = @GiftProductId 
		AND p.[PurchaseProductId] = @PurchaseProductId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

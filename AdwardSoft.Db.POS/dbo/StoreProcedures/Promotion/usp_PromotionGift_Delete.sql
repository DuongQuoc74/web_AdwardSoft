CREATE PROCEDURE [dbo].[usp_PromotionGift_Delete]
	@PromotionId INT ,
	@GiftProductId INT,
	@PurchaseProductId INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[PromotionGift] 
			WHERE [PromotionId] = @PromotionId 
			AND [GiftProductId] = @GiftProductId 
			AND [PurchaseProductId] = @PurchaseProductId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

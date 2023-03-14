CREATE PROCEDURE [dbo].[usp_PromotionCombo_Delete]
	@PromotionId INT ,
	@PromoProductId INT,
	@PurchaseProductId INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[PromotionCombo] 
			WHERE [PromotionId] = @PromotionId 
			AND [PromoProductId] = @PromoProductId 
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
CREATE PROCEDURE [dbo].[usp_PromotionDirect_Delete]
	@PromotionId INT ,
	@PurchaseProductId INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[PromotionDirect] 
			WHERE [PromotionId] = @PromotionId 
			AND [ProductId] = @PurchaseProductId
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

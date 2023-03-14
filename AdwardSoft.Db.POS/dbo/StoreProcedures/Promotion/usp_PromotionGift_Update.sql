CREATE PROCEDURE [dbo].[usp_PromotionGift_Update]
	@PromotionId INT ,
	@PurchaseProductId INT,
	@PurchaseQuantity NUMERIC(13,3),
	@GiftProductId INT,
	@GiftQuantity NUMERIC(13,3)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[PromotionGift]
			SET [GiftQuantity] = @GiftQuantity,
				[PurchaseQuantity] = @PurchaseQuantity
			WHERE [PromotionId] = @PromotionId 
			AND  [PurchaseProductId] = @PurchaseProductId
			AND  [GiftProductId] = @GiftProductId
		COMMIT
	END TRY
	BEGIN CATCH
		SELECT 0;
		RETURN 0;
		ROLLBACK;
		THROW;
	END CATCH
	SELECT 1;
	RETURN 1;
END

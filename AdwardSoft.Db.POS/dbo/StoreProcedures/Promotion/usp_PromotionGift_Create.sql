CREATE PROCEDURE [dbo].[usp_PromotionGift_Create]
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
			INSERT INTO [dbo].[PromotionGift]
					([PromotionId], [PurchaseProductId], [PurchaseQuantity], [GiftProductId], [GiftQuantity])
			VALUES  (@PromotionId , @PurchaseProductId, @PurchaseQuantity, @GiftProductId, @GiftQuantity)
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


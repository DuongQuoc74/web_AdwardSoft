CREATE PROCEDURE [dbo].[usp_PromotionGift_ReadIsExist]
	@PromotionId INT ,
	@GiftProductId INT,
	@PurchaseProductId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS ( SELECT TOP 1 1 FROM [dbo].[PromotionGift] p
						WHERE p.[PromotionId] = @PromotionId 
						AND p.[GiftProductId] = @GiftProductId 
						AND p.[PurchaseProductId] = @PurchaseProductId)
				SELECT 0
			ELSE 
				SELECT 1
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

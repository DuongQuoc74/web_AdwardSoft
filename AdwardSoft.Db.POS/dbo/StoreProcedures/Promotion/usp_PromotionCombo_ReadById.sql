CREATE PROCEDURE [dbo].[usp_PromotionCombo_ReadById]
	@PromotionId INT ,
	@PromoProductId INT,
	@PurchaseProductId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[PromotionCombo] p
		WHERE p.[PromotionId] = @PromotionId 
		AND p.[PromoProductId] = @PromoProductId 
		AND p.[PurchaseProductId] = @PurchaseProductId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

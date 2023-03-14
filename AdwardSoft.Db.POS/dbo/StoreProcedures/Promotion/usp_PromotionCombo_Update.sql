CREATE PROCEDURE [dbo].[usp_PromotionCombo_Update]
	@PromotionId INT ,
	@PurchaseProductId INT,
	@PromoProductId INT,
	@PromoType TINYINT,
	@PromoAmount NUMERIC(16,2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[PromotionCombo]
			SET [PromoType] = @PromoType,
				[PromoAmount] = @PromoAmount
			WHERE [PromotionId] = @PromotionId 
			AND  [PurchaseProductId] = @PurchaseProductId
			AND  [PromoProductId] = @PromoProductId
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

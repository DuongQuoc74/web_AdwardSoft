CREATE PROCEDURE [dbo].[usp_PromotionCombo_Create]
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
			INSERT INTO [dbo].[PromotionCombo]
					([PromotionId], [PurchaseProductId], [PromoProductId], [PromoType], [PromoAmount])
			VALUES  (@PromotionId , @PurchaseProductId, @PromoProductId, @PromoType, @PromoAmount)
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

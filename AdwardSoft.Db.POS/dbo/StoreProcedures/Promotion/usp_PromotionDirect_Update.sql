CREATE PROCEDURE [dbo].[usp_PromotionDirect_Update]
	@PromotionId INT ,
	@PurchaseProductId INT,
	@PromoType TINYINT,
	@PromoAmount NUMERIC(16,2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[PromotionDirect]
			SET [PromoType] = @PromoType,
				[PromoAmount] = @PromoAmount
			WHERE [PromotionId] = @PromotionId 
			AND  [ProductId] = @PurchaseProductId
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
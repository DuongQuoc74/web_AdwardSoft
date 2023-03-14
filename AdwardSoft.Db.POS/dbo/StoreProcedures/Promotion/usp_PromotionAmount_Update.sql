CREATE PROCEDURE [dbo].[usp_PromotionAmount_Update]
	@PromotionId INT ,
	@Idx INT,
	@AmountFrom NUMERIC(16,2),
	@AmountTo NUMERIC(16,2),
	@PromoType TINYINT,
	@PromoAmount NUMERIC(16,2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[PromotionAmount]
			SET [AmountFrom] = @AmountFrom,
				[PromoType] = @PromoType,
				[PromoAmount] = @PromoAmount
			WHERE [PromotionId] = @PromotionId 
			AND  [Idx] = @Idx
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


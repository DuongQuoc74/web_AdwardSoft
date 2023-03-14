CREATE PROCEDURE [dbo].[usp_PromotionAmount_Create]
	@PromotionId INT ,
	@Idx INT,
	@PromoType TINYINT,
	@AmountFrom NUMERIC(16,2),
	@AmountTo NUMERIC(16,2),
	@PromoAmount NUMERIC(16,2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			SELECT @Idx = ISNULL((MAX([Idx]) + 1), 1) FROM [dbo].[PromotionAmount] WHERE [PromotionId] = @PromotionId
			INSERT INTO [dbo].[PromotionAmount]
					([PromotionId], [Idx], [AmountFrom],[AmountTo], [PromoType], [PromoAmount])
			VALUES  (@PromotionId , @Idx, @AmountFrom, @AmountTo, @PromoType, @PromoAmount)
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

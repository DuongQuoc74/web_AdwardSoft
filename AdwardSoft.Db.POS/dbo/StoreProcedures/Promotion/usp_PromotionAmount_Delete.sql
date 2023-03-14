CREATE PROCEDURE [dbo].[usp_PromotionAmount_Delete]
	@PromotionId INT ,
	@Idx INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[PromotionAmount]
			WHERE [PromotionId] = @PromotionId
			AND [Idx] = @Idx
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

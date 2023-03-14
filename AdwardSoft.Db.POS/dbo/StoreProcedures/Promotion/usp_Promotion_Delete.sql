CREATE PROCEDURE [dbo].[usp_Promotion_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[Promotion]
			WHERE [Id] = @Id

			DELETE [dbo].[PromotionAmount]
			WHERE [PromotionId] = @Id

			DELETE [dbo].[PromotionCombo]
			WHERE [PromotionId] = @Id

			DELETE [dbo].[PromotionDirect]
			WHERE [PromotionId] = @Id

			DELETE [dbo].[PromotionGift]
			WHERE [PromotionId] = @Id
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

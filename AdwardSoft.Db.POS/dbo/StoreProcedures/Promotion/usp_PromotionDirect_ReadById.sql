CREATE PROCEDURE [dbo].[usp_PromotionDirect_ReadById]
	@PromotionId INT ,
	@PurchaseProductId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[PromotionDirect] p
		WHERE p.[PromotionId] = @PromotionId 
		AND p.[ProductId] = @PurchaseProductId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

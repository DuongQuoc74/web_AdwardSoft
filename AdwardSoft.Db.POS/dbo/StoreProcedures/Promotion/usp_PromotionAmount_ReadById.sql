CREATE PROCEDURE [dbo].[usp_PromotionAmount_ReadById]
	@PromotionId INT ,
	@Idx INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[PromotionAmount] p
		WHERE p.[PromotionId] = @PromotionId 
		AND p.[Idx] = @Idx
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

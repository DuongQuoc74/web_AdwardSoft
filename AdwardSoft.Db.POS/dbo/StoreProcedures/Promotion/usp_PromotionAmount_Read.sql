CREATE PROCEDURE [dbo].[usp_PromotionAmount_Read]
	@PromotionId int 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[PromotionAmount] p
		WHERE p.[PromotionId] = @PromotionId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

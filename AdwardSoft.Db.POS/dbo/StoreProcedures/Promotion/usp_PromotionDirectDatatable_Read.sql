CREATE PROCEDURE [dbo].[usp_PromotionDirectDatatable_Read]
	@PromotionId int 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.*, ppur.[Name] AS [PurchaseProductName]
		FROM [dbo].[PromotionDirect] p
		INNER JOIN [Product] ppur ON ppur.[Id] = p.[ProductId]
		WHERE p.[PromotionId] = @PromotionId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

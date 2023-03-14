CREATE PROCEDURE [dbo].[usp_ProductData_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
		
			IF NOT EXISTS (SELECT TOP 1 1 FROM [SaleReceiptDetail] WHERE [ProductId] = @Id)
			BEGIN
				DELETE [dbo].[SellingPrice]
			    WHERE [ProductId] = @Id
				DELETE [dbo].[ProductUnitConverter]
			    WHERE [ProductId] = @Id
				DELETE [dbo].[ProductCategory]
				WHERE [ProductId]=@Id
				DELETE [dbo].[Product]
				WHERE [Id] = @Id
			END
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

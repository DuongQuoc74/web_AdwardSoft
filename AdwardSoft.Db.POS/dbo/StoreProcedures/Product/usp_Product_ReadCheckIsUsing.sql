create procedure [dbo].[usp_Product_ReadCheckIsUsing]
@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
		
			IF NOT EXISTS (SELECT TOP 1 1 FROM [CustomerOrderDetail] WHERE [ProductId] = @Id)
			BEGIN
				DELETE [dbo].[CustomerOrderDetail]
			    WHERE [ProductId] = @Id				
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
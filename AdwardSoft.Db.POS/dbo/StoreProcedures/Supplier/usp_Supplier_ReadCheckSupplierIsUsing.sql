CREATE PROCEDURE [dbo].[usp_Supplier_CheckSupplierIsUsing]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[Supplier] AS S
		INNER JOIN [dbo].[SupplierContact] AS SC ON SC.[SupplierId] = S.Id
		WHERE SC.[SupplierId] = @Id

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

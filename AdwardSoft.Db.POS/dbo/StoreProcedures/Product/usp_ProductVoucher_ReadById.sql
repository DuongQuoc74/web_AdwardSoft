CREATE PROCEDURE [dbo].[usp_ProductVoucher_ReadById]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT [Id],
		[Code], [Name],
		[UnitId], [StockId]
		FROM [dbo].[Product]
		WHERE [Id] = @Id AND [Status] = 1
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


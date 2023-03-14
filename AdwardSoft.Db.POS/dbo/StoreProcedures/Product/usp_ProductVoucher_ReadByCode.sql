CREATE PROCEDURE [dbo].[usp_ProductVoucher_ReadByCode]
	@Code CHAR(13)
AS
	SELECT TOP 1 [Id],
		[Code], [Name],
		[UnitId], [StockId]
	FROM [dbo].[Product]
	WHERE [Code] = @Code AND [Status] = 1
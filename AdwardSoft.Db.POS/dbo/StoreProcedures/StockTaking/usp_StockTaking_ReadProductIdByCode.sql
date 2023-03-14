CREATE PROCEDURE [dbo].[usp_StockTaking_ReadProductIdByCode]
	@Code CHAR(13)
AS
	SELECT TOP 1 [Id]
	FROM [dbo].[Product]
	WHERE [Code] = @Code
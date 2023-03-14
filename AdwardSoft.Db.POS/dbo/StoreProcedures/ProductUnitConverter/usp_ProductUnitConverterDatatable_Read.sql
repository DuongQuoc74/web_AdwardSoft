CREATE PROCEDURE [dbo].[usp_ProductUnitConverterDatatable_Read]
	@ProductId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT pu.*, p.[Name] AS [ProductName], u.[Name] AS [UnitName]
		FROM [dbo].[ProductUnitConverter] pu 
		INNER JOIN [dbo].[Product] p ON p.[Id] = pu.[ProductId]
		INNER JOIN [dbo].[Unit] u ON u.[Id] = pu.[UnitId]
		WHERE pu.[ProductId] = @ProductId							   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_ReadById]
	@ProductId INT ,
	@UnitId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[ProductUnitConverter] pu 
		WHERE [ProductId] = @ProductId AND [UnitId] = @UnitId						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

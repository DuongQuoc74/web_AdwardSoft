CREATE PROCEDURE [dbo].[usp_ProductUnitConverter_ReadIsExist]
	@UnitId INT ,
	@ProductId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS ( SELECT TOP 1 1 FROM [dbo].[ProductUnitConverter] p
						WHERE p.[UnitId] = @UnitId 
						AND p.[ProductId] = @ProductId)
				SELECT 0
			ELSE 
				SELECT 1
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

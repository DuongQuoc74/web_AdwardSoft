CREATE PROCEDURE [dbo].[usp_SellingPrice_ReadById]
	@ProductId INT,
	@UnitId INT,
	@Date DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT b.* 
		FROM [SellingPrice] b
		WHERE B.[ProductId] = @ProductId AND B.[UnitId] = @UnitId AND b.[Date] = @Date
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

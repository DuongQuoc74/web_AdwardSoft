CREATE PROCEDURE [dbo].[usp_BeginingStock_ReadHaveData]
	@StockId INT,
	@Year CHAR(4)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1
		FROM [dbo].[BeginingStock] AS BT
		WHERE BT.[StockId] = @StockId AND BT.[Year] = @Year

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
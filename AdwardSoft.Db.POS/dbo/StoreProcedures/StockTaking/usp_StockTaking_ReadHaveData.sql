CREATE PROCEDURE [dbo].[usp_StockTaking_ReadHaveData]
	@StockId INT,
	@Date DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1
		FROM [dbo].[Stocktaking] AS ST
		WHERE ST.[StockId] = @StockId AND ST.[Date] = @Date

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
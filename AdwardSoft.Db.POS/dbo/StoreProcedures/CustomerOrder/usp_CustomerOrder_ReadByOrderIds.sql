CREATE PROCEDURE [dbo].[usp_CustomerOrder_ReadByOrderIds]
	@OrderIds NVARCHAR(MAX) 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		-- Create A Temp Table To Hold OderID => Boots Performance
		CREATE TABLE #OrderIds (OrderID CHAR(32));
		INSERT INTO #OrderIds (OrderID)

		SELECT value FROM STRING_SPLIT(@OrderIds, '|');

		SELECT [CO].*
		FROM [dbo].[CustomerOrder] AS CO
		WHERE [CO].[Id] IN (SELECT OrderID FROM #OrderIds)
		AND [CO].[PaymentStatus] != 1
		ORDER BY [CO].[Date];
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END
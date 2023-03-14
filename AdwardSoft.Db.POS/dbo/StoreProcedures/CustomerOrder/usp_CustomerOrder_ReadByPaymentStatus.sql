CREATE PROCEDURE [dbo].[usp_CustomerOrder_ReadByPaymentStatus]
	@PaymentStatus TINYINT,
	@FromDate SMALLDATETIME,
	@ToDate SMALLDATETIME
AS
BEGIN 
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT CO.* 
		FROM [dbo].[CustomerOrder] CO
		WHERE CO.[PaymentStatus] = @PaymentStatus
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END

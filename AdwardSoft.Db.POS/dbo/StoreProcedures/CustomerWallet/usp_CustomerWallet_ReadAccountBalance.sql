CREATE PROCEDURE [dbo].[usp_CustomerWallet_ReadAccountBalanceByCustomerId]
	@CustomerId INT
AS
BEGIN
SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		DECLARE @AccountBalance NUMERIC(16,2)

		SET @AccountBalance = [dbo].[fn_AccountBalance](@CustomerId)  
		
		SELECT @AccountBalance AS 'AccountBalance'
	END TRY
	
	BEGIN CATCH
		THROW;
	END CATCH
END

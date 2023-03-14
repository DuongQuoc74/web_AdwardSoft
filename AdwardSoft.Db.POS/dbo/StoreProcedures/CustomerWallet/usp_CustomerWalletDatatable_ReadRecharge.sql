CREATE PROCEDURE [dbo].[usp_CustomerWalletDatatable_ReadRecharge]
	@DateFrom DATE,
	@DateTo DATE,
	@AmountFrom NUMERIC(16, 2),
	@AmountTo NUMERIC(16, 2),
	@CustomerId INT
AS
BEGIN
SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		
		IF @AmountTo = 0
		BEGIN
			SET @AmountTo = (SELECT MAX(Amount) FROM CustomerWallet)
		END
		SELECT C.[Date], C.[Amount], C.[Note], C.[Status], C.[Id], B.[BankNo], B.[BankName]
		FROM [dbo].[CustomerWallet] AS C INNER JOIN [dbo].[BankAccount] AS B ON C.[BankAccountId] = B.[Id] 
		WHERE @DateFrom <=  CONVERT(DATE, [Date]) AND  @DateTo >=  CONVERT(DATE, [Date]) AND @AmountFrom <= [Amount] AND @AmountTo >= [Amount] AND [Type] = 0 AND [CustomerId] = @CustomerId
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

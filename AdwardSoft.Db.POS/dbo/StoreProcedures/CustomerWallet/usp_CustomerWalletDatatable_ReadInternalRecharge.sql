﻿CREATE PROCEDURE [dbo].[usp_CustomerWalletDatatable_ReadInternalRecharge]
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

		-- Filter by customer
		IF @CustomerId <> 0
			SELECT C.[Date], C.[Amount], C.[Note], C.[Status], C.[Id], U.[Name] AS CustomerName, [BankNo], [BankName]
			FROM [dbo].[CustomerWallet] AS C INNER JOIN [dbo].[Customer] AS U ON C.[CustomerId] = U.[Id]
											 INNER JOIN [dbo].[BankAccount] AS B ON C.[BankAccountId] = B.[Id]
			WHERE @DateFrom <=  CONVERT(DATE, [Date]) AND  @DateTo >=  CONVERT(DATE, [Date]) AND @AmountFrom <= [Amount] AND @AmountTo >= [Amount] AND [CustomerId] = @CustomerId AND C.[Type] = 0
		ELSE
			SELECT C.[Date], C.[Amount], C.[Note], C.[Status], C.[Id], U.[Name] AS CustomerName, [BankNo], [BankName]
			FROM [dbo].[CustomerWallet] AS C INNER JOIN [dbo].[Customer] AS U ON C.[CustomerId] = U.[Id]
											 INNER JOIN [dbo].[BankAccount] AS B ON C.[BankAccountId] = B.[Id]
			WHERE @DateFrom <=  CONVERT(DATE, [Date]) AND  @DateTo >=  CONVERT(DATE, [Date]) AND @AmountFrom <= [Amount] AND @AmountTo >= [Amount] AND C.[Type] = 0
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

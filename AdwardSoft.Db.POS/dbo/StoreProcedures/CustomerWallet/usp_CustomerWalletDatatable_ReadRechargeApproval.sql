CREATE PROCEDURE [dbo].[usp_CustomerWalletDatatable_ReadRechargeApproval]
AS
BEGIN
SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		
		SELECT C.[Date], C.[Amount], C.[Note], C.[Status], C.[Id], B.[BankNo], B.[BankName], U.[Name] AS CustomerName
		FROM [dbo].[CustomerWallet] AS C INNER JOIN [dbo].[BankAccount] AS B ON C.[BankAccountId] = B.[Id] 
										 INNER JOIN [dbo].[Customer] AS U ON C.[CustomerId] = U.[Id]
		WHERE C.[Type] = 0 AND C.[Status] = 0 AND [BankAccountId] > 0
		ORDER BY C.[Date] DESC
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

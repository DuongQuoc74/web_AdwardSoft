CREATE PROCEDURE [dbo].[usp_CustomerWalletDatatable_ReadCreditApproval]
AS
BEGIN
SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		
		SELECT C.[Date], C.[Amount], C.[Note], C.[Status], C.[Id], U.[Name] AS CustomerName
		FROM [dbo].[CustomerWallet] AS C INNER JOIN [dbo].[Customer] AS U ON C.[CustomerId] = U.[Id]
		WHERE [BankAccountId] = 0 AND C.[Type] = 0 AND C.[Status] = 0
		ORDER BY [Date] DESC
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

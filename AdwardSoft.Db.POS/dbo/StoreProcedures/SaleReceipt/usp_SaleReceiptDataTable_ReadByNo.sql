CREATE PROCEDURE [dbo].[usp_SaleReceiptDataTable_ReadByNo]
	@No	CHAR(15)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT s.*, 
		c.[Name] as [CustomerName], 
		c.[Address] as [CustomerAddress],
		c.[Phone] as [CustomerPhone],
		b.[Name] as [BranchName],
		d.[Name] as [CheckoutCounterName],
		p.[Name] as [PaymentMethodName]
		FROM [SaleReceipt] s
		LEFT JOIN [Customer] c ON s.CustomerId = c.Id
		LEFT JOIN [Branch] b ON s.BranchId = b.Id
		LEFT JOIN [CheckoutCounter] d ON s.CheckoutCounterId = d.Id
		LEFT JOIN [PaymentMethod] p ON s.PaymentMethodId = p.Id
		WHERE s.[No] = @No
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE FUNCTION [dbo].[fn_AccountBalance]
(
	@CustomerId INT
)
RETURNS NUMERIC(16, 2)
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @AmountIn NUMERIC(16, 2) = (
										SELECT ISNULL(SUM([Amount]), 0) 
										FROM [dbo].[CustomerWallet] 
										WHERE [Type] = 0 AND [Status] = 1 AND [CustomerId] = @CustomerId)
	
	DECLARE @AmountOut NUMERIC(16, 2) = (
										SELECT ISNULL(SUM([Amount]), 0) 
										FROM [dbo].[CustomerWallet] 
										WHERE [Type] = 1 AND [Status] = 1 AND [CustomerId] = @CustomerId)
	
	DECLARE @CreditLimit NUMERIC(16 ,2) = (
											SELECT [CreditLimit] 
											FROM [dbo].[Customer] 
											WHERE [Id] = @CustomerId)

	RETURN @AmountIn - @AmountOut;
END
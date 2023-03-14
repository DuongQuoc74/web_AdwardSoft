CREATE FUNCTION [dbo].[fn_CheckTopupCredit]
(
	@Id CHAR(13),
	@CustomerId INT,
	@Type TINYINT,
	@Amount NUMERIC(16, 2)
)
RETURNS TINYINT
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @AmountIn NUMERIC(16, 2) = (
										SELECT ISNULL(SUM([Amount]), 0) 
										FROM [dbo].[CustomerWallet] 
										WHERE [Type] = 0 AND [Status] = 1 AND YEAR([Date]) = YEAR(GETDATE()) AND [CustomerId] = @CustomerId AND [Id] <> @Id)
	
	DECLARE @AmountOut NUMERIC(16, 2) = (
										SELECT ISNULL(SUM([Amount]), 0) 
										FROM [dbo].[CustomerWallet] 
										WHERE [Type] = 1 AND [Status] = 1 AND YEAR([Date]) = YEAR(GETDATE()) AND [CustomerId] = @CustomerId AND [Id] <> @Id)
	
	DECLARE @CreditLimit NUMERIC(16 ,2) = (
											SELECT [CreditLimit] 
											FROM [dbo].[Customer] 
											WHERE [Id] = @CustomerId)

	IF @Type = 0
		SET @AmountIn = @AmountIn + @Amount 
	ELSE
		SET @AmountOut = @AmountOut + @Amount 

	IF (@AmountIn - @AmountOut) < @CreditLimit 
		RETURN 1;

	RETURN 0;

END
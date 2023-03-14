CREATE PROCEDURE [dbo].[usp_MembershipScore_ReadById]
	@Id INT, --Customer Id
	@DateFrom DATE,
	@DateTo DATE
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	DECLARE @Point NUMERIC(9,0) = 0
		--Earn
	SELECT @Point = ISNULL(SUM(Point),0)
	FROM [dbo].[MembershipScore] a LEFT JOIN [dbo].[SaleReceipt] b ON a.[SaleReceiptId] = b.Id
	WHERE a.[CustomerId] = @Id AND (CONVERT(DATE,b.[Date]) BETWEEN @DateFrom AND @DateTo)
	AND a.[Type] = 1
	--AND [Year] =  CAST(YEAR(GETDATE()) AS CHAR(4)) 

	--Exchange
	SELECT @Point = @Point - ISNULL(SUM(Point),0)
	FROM [dbo].[MembershipScore] a LEFT JOIN [dbo].[SaleReceipt] b ON a.[SaleReceiptId] = b.Id
	--WHERE a.[CustomerId] = @Id AND (a.[Year] = 2021 OR (a.[Year] = 2022 AND MONTH(b.[Date]) = 1))
	WHERE a.[CustomerId] = @Id AND (CONVERT(DATE,b.[Date]) BETWEEN @DateFrom AND @DateTo)
	AND a.[Type] = 0

	SELECT @Point
END
CREATE PROCEDURE [dbo].[usp_DashboardSumary_Read]
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

	DECLARE @NewCustomer INT, @TotalOrder INT
	DECLARE @TotalSale NUMERIC(16,2), @TotalSaleMonth NUMERIC(16,2),
	@TotalDayCashier1 NUMERIC(16,2), @TotalDayCashier2 NUMERIC(16,2),
	@TotalCashier1 NUMERIC(16,2), @TotalCashier2 NUMERIC(16,2)

	--New customers
	SELECT @NewCustomer = COUNT([Id]) FROM [dbo].[Customer]
	WHERE [CreateDate] = CAST(GETDATE() AS DATE)

	--Total Orders
	SELECT @TotalOrder = COUNT([Id]) FROM [dbo].[SaleReceipt]
	WHERE CAST([CreateDate] AS DATE) = CAST(GETDATE() AS DATE)

	--Total Sales
	SELECT @TotalSale = SUM([TotalAmount]) FROM [dbo].[SaleReceipt]
	WHERE CAST([CreateDate] AS DATE) = CAST(GETDATE() AS DATE) 

	--Total Sales
	SELECT @TotalSaleMonth = SUM([TotalAmount]) FROM [dbo].[SaleReceipt]
	WHERE MONTH([CreateDate]) = MONTH(GETDATE())  AND YEAR([CreateDate]) = YEAR(GETDATE())
	
	--Total by Cashier Desk
	--by day
	SELECT @TotalDayCashier1 = SUM(
		CASE  
             WHEN [EmployeeId] = 22 THEN [TotalAmount] 
              ELSE 0 
           END
	),
	@TotalDayCashier2 = SUM(
		CASE  
             WHEN [EmployeeId] = 24 THEN [TotalAmount] 
              ELSE 0 
           END
	) 
	FROM [dbo].[SaleReceipt]
	WHERE CAST([CreateDate] AS DATE) = CAST(GETDATE() AS DATE) 
	--by month
	SELECT @TotalCashier1 = SUM(
		CASE  
             WHEN [EmployeeId] = 22 THEN [TotalAmount] 
              ELSE 0 
           END
	),
	@TotalCashier2 = SUM(
		CASE  
             WHEN [EmployeeId] = 24 THEN [TotalAmount] 
              ELSE 0 
           END
	) 
	FROM [dbo].[SaleReceipt]
	WHERE MONTH([CreateDate]) = MONTH(GETDATE())  AND YEAR([CreateDate]) = YEAR(GETDATE())

	--Sumary Report
	SELECT @NewCustomer AS [NewCustomer],
	@TotalOrder AS [TotalOrder],
	@TotalSale AS [TotalSale],
	@TotalSaleMonth AS [TotalSaleMonth],
	@TotalCashier1 AS [TotalCashier1],
	@TotalCashier2 AS [TotalCashier2],
	@TotalDayCashier1 AS [TotalDayCashier1],
	@TotalDayCashier2 AS [TotalDayCashier2]
END
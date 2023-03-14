CREATE PROCEDURE [dbo].[usp_DashboardPie_Read]
@Type TINYINT = 1
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

	--Products
	IF(@Type = 1)
	BEGIN
		SELECT TOP 5 MAX(p.[Name]) AS [Name], SUM(d.[Amount]) AS [Value]
		FROM [dbo].[SaleReceiptDetail] d 
		INNER JOIN [dbo].[SaleReceipt] s ON s.[Id] = d.[SaleReceiptId]
		INNER JOIN [dbo].[Product] p ON d.[ProductId] = p.[Id]
		WHERE MONTH(s.[CreateDate]) = MONTH(GETDATE())
		GROUP BY d.[ProductId]
		ORDER BY SUM(d.[Amount]) DESC
	END
	ELSE 
	BEGIN
		IF(@Type = 2) --Customers
			SELECT TOP 5 MAX(c.[Name]) AS [Name], SUM(s.[TotalAmount]) AS [Value]
			FROM [dbo].[SaleReceipt] s
			INNER JOIN [dbo].[Customer] c ON s.[CustomerId] = c.[Id]
			WHERE MONTH(s.[CreateDate]) = MONTH(GETDATE()) AND c.[Id] <> 543 --Default customer
			GROUP BY s.[CustomerId]
			ORDER BY SUM(s.[TotalAmount]) DESC
		ELSE -- Categories
			SELECT TOP 5 MAX(c.[Name]) AS [Name], SUM(d.[Amount]) AS [Value]
			FROM [dbo].[SaleReceiptDetail] d 
			INNER JOIN [dbo].[SaleReceipt] s ON s.[Id] = d.[SaleReceiptId]
			INNER JOIN [dbo].[Product] p ON d.[ProductId] = p.[Id]
			INNER JOIN [dbo].[ProductCategory] pc ON  pc.[ProductId] = p.[Id]
			INNER JOIN [dbo].[Category] c ON c.[Id] = pc.[CategoryId]
			WHERE MONTH(s.[CreateDate]) = MONTH(GETDATE())
			GROUP BY c.[Id]
			ORDER BY SUM(d.[Amount]) DESC
	END
	
END
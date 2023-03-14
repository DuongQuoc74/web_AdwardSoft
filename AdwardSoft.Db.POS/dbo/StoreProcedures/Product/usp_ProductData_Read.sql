CREATE PROCEDURE [dbo].[usp_ProductData_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT P.*,
		SP.[WholesalePrice] AS [WholesalePrice],
		SP.[RetailPrice] AS [RetailPrice],
		C.[Id] AS CategoryId, C.[Name] AS CategoryName
		FROM [dbo].[Product] AS P INNER JOIN [dbo].[ProductCategory] AS PC ON P.[Id] = PC.[ProductId]
		LEFT JOIN [dbo].[SellingPrice] AS SP ON P.[Id] = SP.[ProductId] AND P.[UnitId] = SP.[UnitId]
		LEFT JOIN [dbo].[Category] AS C ON PC.[CategoryId] = C.[Id]
		WHERE 
			PC.[IsDefault] = 1 AND
			SP.[Date]=(SELECT TOP 1 S.[Date] FROM [dbo].[SellingPrice] AS S WHERE S.[ProductId]=P.[Id] AND P.[UnitId] = SP.[UnitId] AND S.[Date] <= GETDATE() ORDER BY S.[Date] DESC) 
		ORDER BY P.[Name] ASC
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
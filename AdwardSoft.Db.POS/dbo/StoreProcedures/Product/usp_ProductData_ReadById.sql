CREATE PROCEDURE [dbo].[usp_ProductData_ReadById]
@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT P.*,C.[Id] AS CategoryId, C.[Name] AS CategoryName, U.[Name] AS UnitName
		FROM [dbo].[Product] AS P
		INNER JOIN [dbo].[ProductCategory] AS PC ON P.[Id] = PC.[ProductId]		
		INNER JOIN [dbo].[Category] AS C ON PC.[CategoryId] = C.[Id]
		INNER JOIN [dbo].[Unit] AS U ON P.[UnitId] = U.[Id]
		--LEFT JOIN  [dbo].[SellingPrice] AS SP ON  P.[Id] = SP.[ProductId] AND 
		--	SP.[Date] = (SELECT TOP 1 S.[Date] FROM [dbo].[SellingPrice] AS S 
		--	WHERE S.[ProductId] = P.[Id] AND S.[UnitId] = P.[UnitId] 
		--	ORDER BY [DATE] DESC) AND SP.[UnitId] = P.[UnitId]
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

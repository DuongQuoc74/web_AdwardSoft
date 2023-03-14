CREATE PROCEDURE [dbo].[usp_BeginingStockDataTable_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@Year CHAR(4),
	@StockId INT = 0,
	@ProductId INT = 0,
	@BranchId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @PageSize INT, 
											  @PageSkip INT, 
											  @SearchBy NVARCHAR(50),
											  @Year CHAR(4),
											  @StockId INT,
											  @ProductId INT,
											  @BranchId INT '

		SET @SqlStr = N' SELECT BT.*, P.[Name] AS [ProductName], P.[Code] AS [ProductCode], P.[Image] AS [ProductImage],
						 	 S.[Name] AS [StockName],  U.[Name] AS [UnitName], BT.[Quantity], COUNT(*) OVER() AS [Count]
						 FROM [dbo].[BeginingStock] AS BT
						 INNER JOIN [dbo].[Product] AS P ON BT.[ProductId] = P.[Id]
						 INNER JOIN [dbo].[Stock] AS S ON BT.[StockId] = S.[Id]
						 INNER JOIN [dbo].[Unit] AS U ON BT.[UnitId] = U.[Id]
						 WHERE BT.[Year] = @Year'

		IF(@BranchId <> 0)
			SET @SqlStr = @SqlStr + N' AND S.[BranchId] = @BranchId '

		IF(@StockId <> 0)
			SET @SqlStr = @SqlStr + N' AND BT.[StockId] = @StockId '

		IF(@ProductId <> 0)
			SET @SqlStr = @SqlStr + N' AND BT.[ProductId] = @ProductId '

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND P.[Name] LIKE ''%''+@SearchBy+''%'' OR S.[Name] LIKE ''%''+@SearchBy+''%'' OR U.[Name] LIKE ''%''+@SearchBy+''%'' '

		SET @SqlStr= @SqlStr + N' ORDER BY P.[Name], U.[Name] 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @Year,
						   @StockId,
						   @ProductId,
						   @BranchId 

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


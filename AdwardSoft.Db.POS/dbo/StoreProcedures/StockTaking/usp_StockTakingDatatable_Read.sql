CREATE PROCEDURE [dbo].[usp_StockTakingDatatable_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
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
											  @DateFrom DATETIME,
											  @DateTo DATETIME,
											  @StockId INT,
											  @ProductId INT, 
											  @BranchId INT '

		SET @SqlStr = N' SELECT ST.*, P.[Name] AS [ProductName], P.[Code] AS [ProductCode], 
						 	 S.[Name] AS [StockName],  U.[Name] AS [UnitName], ST.[Quantity], COUNT(*) OVER() AS [Count]
						 FROM [dbo].[Stocktaking] AS ST
						 INNER JOIN [dbo].[Product] AS P ON ST.[ProductId] = P.[Id]
						 INNER JOIN [dbo].[Stock] AS S ON ST.[StockId] = S.[Id]
						 INNER JOIN [dbo].[Unit] AS U ON ST.[UnitId] = U.[Id]
						 WHERE @DateFrom <=  CONVERT(DATE, ST.[Date]) AND  @DateTo >=  CONVERT(DATE, ST.[Date]) '

		IF(@BranchId <> 0)
			SET @SqlStr = @SqlStr + N' AND S.[BranchId] = @BranchId '

		IF(@StockId <> 0)
			SET @SqlStr = @SqlStr + N' AND ST.[StockId] = @StockId '

		IF(@ProductId <> 0)
			SET @SqlStr = @SqlStr + N' AND ST.[ProductId] = @ProductId '

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND ( P.[Name] LIKE ''%''+@SearchBy+''%'' OR S.[Name] LIKE ''%''+@SearchBy+''%'' OR P.[Name] LIKE ''%''+@SearchBy+''%'' OR P.[Code] LIKE ''%''+@SearchBy+''%'' ) '

		SET @SqlStr= @SqlStr + N' ORDER BY ST.[Date] DESC 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo,
						   @StockId,
						   @ProductId,
						   @BranchId 

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END



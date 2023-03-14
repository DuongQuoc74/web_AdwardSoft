CREATE PROCEDURE [dbo].[usp_ProductPrint_Read]
	@PageSize INT,
	@PageSkip INT,
	@SearchBy NVARCHAR(150),
	@CategoryIds NVARCHAR(MAX),
	@Type INT = 1
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@Count INT,

		@ParamList NVARCHAR(2000) = ' @pageSize INT, 
											  @PageSkip INT, 
											  @searchBy NVARCHAR(50),
											  @CategoryIds NVARCHAR(MAX) ',
		@ParamList2 NVARCHAR(2000) = '@searchBy NVARCHAR(50),
											  @CategoryIds NVARCHAR(MAX) '
		
		SELECT @Count = COUNT(*) FROM STRING_SPLIT(@CategoryIds, ',');
		SET @SqlStr = '1=1'
		IF(@searchBy <> 'NULL')
		BEGIN
			IF(LEFT(@SearchBy,1) = '*')
				SET @SearchBy = REPLACE(@SearchBy,'*','%')
			SET @SearchBy = @SearchBy + '%'
			SET @SqlStr = N' [Code] LIKE N'''+@SearchBy+''' OR [Name] LIKE N'''+@SearchBy +'''';
		END

		IF(@Type = 0)
			SET @SqlStr = N' SELECT P.[Id], P.[Code], P.[Name], SP.[RetailPrice] AS [RetailPrice1],  c.[Name] as CategoryName,
						 U.[Name] AS UnitName1
						 FROM (SELECT * FROM [dbo].[Product] WHERE '+ @SqlStr + ') AS P, 
						 [dbo].[ProductCategory] AS PC,
						 (SELECT * FROM (SELECT * , ROW_NUMBER() OVER(PARTITION BY ProductId, UnitId ORDER BY [date] DESC ) AS RN
						 FROM [dbo].[SellingPrice]) AS SB
							WHERE RN = 1) AS SP,
						 [dbo].[Category] AS C,
						 [dbo].[Unit] AS U
						 WHERE P.[Id] = PC.[ProductId] AND P.[Id] = SP.[ProductId] AND PC.[CategoryId] = C.[Id] AND P.[UnitId] = U.[Id]
						 AND P.[Id] = SP.[ProductId] AND P.[UnitId] = SP.[UnitId]';
		ELSE
			SET @SqlStr = N' SELECT 
				P.[Id], P.[Code], MAX(P.[Name]) AS [Name], 
				MAX(CASE WHEN SPU.[s_index] = 1 THEN SP.RetailPrice END) AS RetailPrice1,
				MAX(CASE WHEN SPU.[s_index] = 2 THEN SP.RetailPrice END) AS RetailPrice2,
				MAX(CASE WHEN SPU.[s_index] = 3 THEN SP.RetailPrice END) AS RetailPrice3,
				MAX(CASE WHEN SPU.[s_index] = 1 THEN U.[Name] END) AS UnitName1,
				MAX(CASE WHEN SPU.[s_index] = 2 THEN U.[Name] END) AS UnitName2,
				MAX(CASE WHEN SPU.[s_index] = 3 THEN U.[Name] END) AS UnitName3,
				MAX(c.[Name]) as CategoryName
						 FROM (SELECT * FROM [dbo].[Product] WHERE '+ @SqlStr + ') AS P, 
						 [dbo].[ProductCategory] AS PC,
						 (SELECT * FROM (SELECT * , ROW_NUMBER() OVER(PARTITION BY ProductId, UnitId ORDER BY [date] DESC ) AS RN
						 FROM [dbo].[SellingPrice]) AS SB
							WHERE RN = 1) AS SP,
						  (SELECT [ProductId],[UnitId], s_index = ROW_NUMBER() OVER(PARTITION BY [ProductId] ORDER BY [ProductId])
						  FROM [dbo].[SellingPrice] WHERE [Date] <= GETDATE() GROUP BY [ProductId],[UnitId]) AS SPU,
						 [dbo].[Category] AS C,
						 [dbo].[Unit] AS U
						 WHERE P.[Id] = PC.[ProductId] AND PC.[CategoryId] = C.[Id] 
						 AND P.[Id] = SPU.[ProductId] AND SPU.[UnitId] = U.[Id] AND SP.[ProductId] = SPU.ProductId
						 AND SP.[UnitId] = SPU.[UnitId]';
		
		IF(0 NOT IN (SELECT * FROM STRING_SPLIT(@CategoryIds, ',')) AND @Count > 0)
			SET @SqlStr = @SqlStr + N' AND PC.[CategoryId] IN (SELECT * FROM STRING_SPLIT(@CategoryIds, '',''))';

		IF(@Type = 1 OR @Type = 2)
			SET @SqlStr = @SqlStr + N' GROUP BY P.[Id], P.[Code] ';

		IF(@PageSize > 0)
		BEGIN 
			SET @SqlStr = @SqlStr + N' ORDER BY P.[Code] ASC 
									   OFFSET @PageSkip ROWS 
									   FETCH NEXT @pageSize ROWS ONLY;'
			print @SqlStr
			EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @CategoryIds
		END
		ELSE
		BEGIN
			SET @SqlStr = @SqlStr + N' ORDER BY P.[Code] ASC'
			print @SqlStr
			EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList2,
						   @searchBy,
						   @CategoryIds
		END
		
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_ProductDatatable_Read]
	@PageSize INT,
	@PageSkip INT,
	@SearchBy NVARCHAR(150),
	@CategoryIds NVARCHAR(MAX)
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
		--print @SearchBy

		SET @SqlStr = N' SELECT DISTINCT P.*,U.[Name] AS UnitName, SP.*, COUNT(P.[Id]) OVER() AS [Count], [dbo].[fn_ConcatCategory](p.[Id]) AS [CategoryName]
						 FROM (SELECT * FROM [dbo].[Product] WHERE '+ @SqlStr + ') AS P
						 INNER JOIN [dbo].[ProductCategory] AS PC ON P.[Id] = PC.[ProductId]
						 LEFT JOIN [dbo].[SellingPrice] AS SP ON  P.[Id] = SP.[ProductId] AND SP.[UnitId] = P.[UnitId] AND SP.[Date] = ( SELECT TOP 1 S.[Date] FROM [dbo].[SellingPrice] AS S WHERE S.[ProductId] = P.[Id] AND S.[UnitId] = P.[UnitId]  ORDER BY S.[Date] DESC)
						 INNER JOIN[dbo].[Category] AS C ON PC.[CategoryId] = C.[Id]
						 INNER JOIN[dbo].[Unit] AS U ON P.[UnitId] = U.[Id]
						 WHERE  1 = 1  ';
		
		IF(0 NOT IN (SELECT * FROM STRING_SPLIT(@CategoryIds, ',')) AND @Count >= 1)
			SET @SqlStr = @SqlStr + N' AND PC.[CategoryId] IN (SELECT * FROM STRING_SPLIT(@CategoryIds, '',''))';


		--IF(@searchBy <> 'NULL')
		--	SET @SqlStr = @SqlStr + N' AND P.[Code] LIKE ''%''+@SearchBy+''%''';

		IF(@PageSize > 0)
		BEGIN 
			SET @SqlStr = @SqlStr + N' ORDER BY P.[Name] ASC 
									   OFFSET @PageSkip ROWS 
									   FETCH NEXT @pageSize ROWS ONLY;'
			--print @SqlStr
			EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @CategoryIds
		END
		ELSE
		BEGIN
			SET @SqlStr = @SqlStr + N' ORDER BY P.[Name]  ASC'
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
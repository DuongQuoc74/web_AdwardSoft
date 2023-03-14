CREATE PROCEDURE [dbo].[usp_SelectMultiCategory_ReadByCategoryIds]
@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@listCategory NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),@Count INT ,
				@ParamList NVARCHAR(2000) = ' @pageSize INT, @pageSkip INT, @searchBy NVARCHAR(50),@listCategory NVARCHAR(50) '

		SELECT @Count=COUNT(*) FROM STRING_SPLIT(@listCategory,',');

		SET @SqlStr=N'SELECT P.*,SP.*,C.[Id] AS CategoryId, C.[Name] AS categoryName,COUNT(*) OVER() AS [Count]
						FROM [dbo].[Product] AS P, [dbo].[ProductCategory] AS PC,
						[dbo].[SellingPrice] AS SP,[dbo].[Category] AS C
						WHERE P.[Id] = PC.[ProductId] AND P.[Id] = SP.[ProductId] AND PC.[CategoryId] = C.[Id]
						AND SP.[Date]=(SELECT TOP 1 S.[Date] FROM [dbo].[SellingPrice] AS S WHERE S.[ProductId]=P.[Id])';
		IF(0 NOT IN (SELECT * FROM STRING_SPLIT(@listCategory,',')) AND @Count>=1)
		BEGIN
			SET @SqlStr=@SqlStr+N' AND PC.[CategoryId] IN (SELECT * FROM STRING_SPLIT(@listCategory,'',''))';
		END
		IF(@searchBy <> 'NULL')
		begin
			SET @SqlStr = @SqlStr+  N' AND P.[Name] LIKE ''%''+@SearchBy+''%'' OR P.[Code] LIKE ''%''+@SearchBy+''%'' ';
		end

			 SET @SqlStr=@SqlStr+N' ORDER BY P.[Name] ASC 
									OFFSET @pageSkip ROWS 
									FETCH NEXT @pageSize ROWS ONLY; '

		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @pageSize,
						   @pageSkip ,
						   @searchBy,
						   @listCategory
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_ReportStock_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
	@BranchId INT
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
											  @BranchId INT '

		SET @SqlStr = N' SELECT P.[Id] AS [ProductId], P.[Code] AS [Barcode], P.[Name], U.[Name] AS [Unit]
						, [dbo].[fn_BeginingStock](P.[Id], @DateFrom, @DateTo, @BranchId) AS [BeginingStock]
						, [dbo].[fn_ImportStock](P.[Id], @DateFrom, @DateTo, @BranchId) AS [ImportStock]
						, [dbo].[fn_ExportStock](P.[Id], @DateFrom, @DateTo, @BranchId) AS [ExportStock]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[Product] AS P
						INNER JOIN [dbo].[Unit] AS U ON P.[UnitId] = U.[Id]
						WHERE 1 = 1 '



		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND P.[Name] LIKE ''%''+@SearchBy+''%'' OR P.[Code] LIKE ''%''+@SearchBy+''%'' '

		SET @SqlStr= @SqlStr + N' ORDER BY P.[Name] 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo,
						   @BranchId

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
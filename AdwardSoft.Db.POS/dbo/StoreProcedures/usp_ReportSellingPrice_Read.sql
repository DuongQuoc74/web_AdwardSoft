CREATE PROCEDURE [dbo].[usp_ReportSellingPrice_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
	@ReceivingReportId VARCHAR(32),	
	@SupplierId INT,
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
											  @ReceivingReportId VARCHAR(32),	
											  @SupplierId INT,
											  @BranchId INT '		
					
		SET @SqlStr = N' SELECT P.[Id] AS [ProductId], R.[Id] AS [Id], P.[Code] AS [Barcode] , P.[Name], S.[Name] AS [Supplier]
						, U.[Name] AS [Unit], ISNULL(SP.[RetailPrice], 0) AS [SellingPrice]
						, [dbo].[fn_AvgPrice](P.[Id], S.[Id], @BranchId) AS [AvgPrice]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[ReceivingReport] R
						INNER JOIN [dbo].[Supplier] S ON S.[Id] = R.[SupplierId] 		
						INNER JOIN [dbo].[ReceivingReportDetail] RD ON R.[Id] = RD.[ReceivingReportId]		
						INNER JOIN [dbo].[Product]  P ON P.[Id] = RD.[ProductId]
						INNER JOIN [dbo].[Unit]  U ON P.[UnitId] = U.[Id]
						LEFT JOIN [dbo].[SellingPrice]  SP ON  P.[Id] = SP.[ProductId] AND SP.[UnitId] = P.[UnitId] 
						AND SP.[Date] = ( SELECT TOP 1 S.[Date] FROM [dbo].[SellingPrice] AS S WHERE S.[ProductId] = P.[Id] AND S.[UnitId] = P.[UnitId]  ORDER BY S.[Date] DESC)
						WHERE R.[BranchId] = @BranchId  '

		IF(@ReceivingReportId <> 'NULL')
		BEGIN
			SET @SqlStr =  @SqlStr + N' AND R.[Id] = @ReceivingReportId '
		END
		ELSE
		BEGIN
			SET @SqlStr =  @SqlStr + N' AND  @DateFrom <=  CONVERT(DATE, R.[CreateDate]) AND  @DateTo >=  CONVERT(DATE, R.[CreateDate]) '

			IF(@SupplierId <> 0)
			BEGIN
				SET @SqlStr =  @SqlStr + N' AND S.[Id] = @SupplierId '
			END			
		END



			
		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND P.[Code] LIKE ''%''+@SearchBy+''%'' OR P.[Name] LIKE ''%''+@SearchBy+''%'' '

		SET @SqlStr= @SqlStr + N' GROUP BY P.[Id], P.[Code], P.[Name], U.[Name], SP.[RetailPrice], S.[Name], S.[Id], R.[Id]
								  ORDER BY P.[Name] DESC 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo,
						   @ReceivingReportId,	
						   @SupplierId,
						   @BranchId

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

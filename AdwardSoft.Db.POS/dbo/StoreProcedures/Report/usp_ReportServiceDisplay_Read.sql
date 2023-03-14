CREATE PROCEDURE [dbo].[usp_ReportServiceDisplay_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
	@SupplierId INT
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
											  @SupplierId INT '

		SET @SqlStr = N' SELECT SD.*, S.[Name] AS [Supplier]
						, COUNT(*) OVER() AS [Count]
						, COUNT(S.[Id]) OVER() AS [TotalSupplier]
						, SUM(SD.[Fee]) OVER() AS [TotalAmount]
						FROM [dbo].[ServiceDisplay] SD
						INNER JOIN [dbo].[Supplier] S ON S.[Id] = SD.[SupplierId] 
						WHERE (
						   (@DateFrom < = SD.[DateFrom] AND  @DateTo >=  SD.[DateFrom])
						OR (@DateFrom <= SD.[DateTo] AND  @DateTo >=  SD.[DateTo])
						OR (@DateFrom < SD.[DateFrom] AND  @DateTo <  SD.[DateTo]) )  '


		IF(@SupplierId <> 0)
			SET @SqlStr =  @SqlStr + N' AND S.[Id] = @SupplierId '			

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND SD.[Name] LIKE ''%''+@SearchBy+''%'' OR S.[Name] LIKE ''%''+@SearchBy+''%'' '

		SET @SqlStr= @SqlStr + N' ORDER BY SD.[DateFrom] DESC 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo,
						   @SupplierId

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
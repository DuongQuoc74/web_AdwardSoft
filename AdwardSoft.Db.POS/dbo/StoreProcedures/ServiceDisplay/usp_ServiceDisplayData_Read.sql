CREATE PROCEDURE [dbo].[usp_ServiceDisplayData_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
	@SupplierId INT,
	@Status TINYINT
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
											  @SupplierId INT, 
											  @Status TINYINT '

		SET @SqlStr = N' SELECT SD.*, S.[Name] AS [Supplier]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[ServiceDisplay] SD
						INNER JOIN [dbo].[Supplier] S ON S.[Id] = SD.[SupplierId] 
						WHERE (
						   (@DateFrom < = SD.[DateFrom] AND  @DateTo >=  SD.[DateFrom])
						OR (@DateFrom <= SD.[DateTo] AND  @DateTo >=  SD.[DateTo])
						OR (@DateFrom < SD.[DateFrom] AND  @DateTo <  SD.[DateTo]) )  '


		IF(@SupplierId <> 0)
			SET @SqlStr =  @SqlStr + N' AND S.[Id] = @SupplierId '
			
		IF(@Status <> 0)
			SET @SqlStr =  @SqlStr + N' AND CONVERT(DATE, GETDATE())  >  SD.[DateTo] '

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
						   @SupplierId,
						   @Status

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

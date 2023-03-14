CREATE PROCEDURE [dbo].[usp_CategoryDatatable_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '@pageSize INT, 
											  @pageSkip INT, 
											  @SearchBy NVARCHAR(50) '
			
		SET @SqlStr= N' SELECT C.*, COUNT(*) OVER() AS [Count]
					    FROM [dbo].[Category] AS C
						WHERE 1 = 1 ';
		IF(@searchBy <> 'NULL')
			SET @SqlStr = @SqlStr + N' AND C.[Name] LIKE ''%''+@SearchBy+''%'' '
		
		SET @SqlStr = @SqlStr + N' ORDER BY C.[Name] ASC 
							       OFFSET @PageSkip ROWS 
							       FETCH NEXT @PageSize ROWS ONLY; '

		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @pageSize,
						   @pageSkip ,
						   @SearchBy	 
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


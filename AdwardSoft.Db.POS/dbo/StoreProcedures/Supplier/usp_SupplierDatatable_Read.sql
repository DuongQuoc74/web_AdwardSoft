CREATE PROCEDURE [dbo].[usp_SupplierDatatable_Read]
	@pageSize INT,
	@pageSkip INT,
	@searchBy NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @pageSize INT, @pageSkip INT, @searchBy NVARCHAR(50) '
		
		SET @SqlStr =  ' SELECT S.* ,COUNT(*) OVER() AS [Count]
					     FROM [dbo].[Supplier] S						 
						 WHERE 1 = 1 '

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr =  ' AND S.[Name] LIKE ''%''+@searchBy+''%'' OR S.[Tel] LIKE ''%''+@searchBy+''%'' '
		END
		

	    SET @SqlStr = @SqlStr + ' ORDER BY S.[Name] ASC 
								  OFFSET @pageSkip ROWS 
								  FETCH NEXT @pageSize ROWS ONLY;' 

		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @pageSize,
						   @pageSkip ,
						   @searchBy	 
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

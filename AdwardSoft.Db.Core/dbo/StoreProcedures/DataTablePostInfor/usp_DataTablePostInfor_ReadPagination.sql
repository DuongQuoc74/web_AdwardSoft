CREATE PROCEDURE [dbo].[usp_DataTablePostInfor_ReadPagination]
	@pageSize INT,
	@skip INT,
	@searchBy NVARCHAR(50)
AS 
	BEGIN
	
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		
		DECLARE @SqlStr1 NVARCHAR(MAX),
				@SqlStr2 NVARCHAR(MAX), 
				@ParamList  NVARCHAR(2000) = '@pageSize INT,
											  @skip INT,
											  @searchBy NVARCHAR(50)'
		
		SET @SqlStr1 = N'DECLARE @Total INT
						SELECT @Total = COUNT(p.[Id])
						FROM [Post] p
						INNER JOIN [PostSEO] ps ON p.[Id] = ps.[PostId]	
						WHERE 1 = 1 ';
		
		
		SET @SqlStr2 = N'SELECT p.* , @Total AS [Count]
						FROM [Post] p
						INNER JOIN [PostSEO] ps ON p.[Id] = ps.[PostId]	
						WHERE 1 = 1 ';

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND p.[Title] LIKE ''%''+@searchBy+''%'' '
			SET @SqlStr2 = @SqlStr2 + N'AND p.[Title] LIKE ''%''+@searchBy+''%'' '
		END

		SET @SqlStr1 = @SqlStr1 + @SqlStr2 + N' ORDER BY p.[Title]	
												OFFSET @skip ROWS 
												FETCH NEXT @pageSize ROWS ONLY; '
		EXEC SP_EXECUTESQL @SqlStr1, 
						   @ParamList,
						   @pageSize,
						   @skip,
						   @searchBy
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

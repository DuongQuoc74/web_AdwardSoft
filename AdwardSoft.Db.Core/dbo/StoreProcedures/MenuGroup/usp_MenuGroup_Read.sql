CREATE PROCEDURE [dbo].[usp_MenuGroup_Read]
	@pageSize INT,
	@skip INT,
	@searchBy NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr2 NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '@pageSize INT,
											  @skip INT,
											  @searchBy NVARCHAR(50) '
		
		SET @SqlStr2 = N'SELECT MG.*, COUNT(MG.[Id]) OVER() AS [Count]
						FROM [dbo].[MenuGroup] AS MG	
						WHERE 1 = 1 ';

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr2 = @SqlStr2 + N'AND MG.[Name] LIKE ''%''+@searchBy+''%'' '
		END

		SET @SqlStr2 = @SqlStr2 + N' ORDER BY MG.[Position]	
									 OFFSET @skip ROWS 
									 FETCH NEXT @pageSize ROWS ONLY; '
		EXEC SP_EXECUTESQL @SqlStr2, 
						   @ParamList,
						   @pageSize,
						   @skip,
						   @searchBy
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
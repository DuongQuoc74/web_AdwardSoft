CREATE PROCEDURE [dbo].[usp_ScoreConfigurationDatatable_Read]
	@pageSize INT,
	@pageSkip INT,
	@searchBy NVARCHAR(50),
	@status INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = ' @pageSize INT, @pageSkip INT, @searchBy NVARCHAR(50),@status INT '
		IF(@status=0)
		BEGIN
			SET @SqlStr= N'SELECT S.* ,COUNT(*) OVER() AS [Count]
					    FROM [dbo].[ScoreConfiguration] AS S
						WHERE 1=1 ';
		END
		ELSE IF(@status=1)
		BEGIN
			SET @SqlStr= N'SELECT S.* ,COUNT(*) OVER() AS [Count]
					    FROM [dbo].[ScoreConfiguration] AS S
						WHERE S.[EffectiveDate]>=GETDATE() ';
		END
		ELSE
		BEGIN
			SET @SqlStr= N'SELECT S.* ,COUNT(*) OVER() AS [Count]
					    FROM [dbo].[ScoreConfiguration] AS S
						WHERE S.[EffectiveDate]<GETDATE() ';
		END
		IF(@searchBy <> 'null')
		BEGIN
			SET @SqlStr = @SqlStr+ N' AND convert(NVARCHAR,S.[EffectiveDate],103) LIKE ''%''+@searchBy+''%'' '
		END
		
			 SET @SqlStr=@SqlStr+N' ORDER BY S.[EffectiveDate] DESC
									OFFSET @pageSkip ROWS 
									FETCH NEXT @pageSize ROWS ONLY; '

		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @pageSize,
						   @pageSkip ,
						   @searchBy,
						   @status
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END



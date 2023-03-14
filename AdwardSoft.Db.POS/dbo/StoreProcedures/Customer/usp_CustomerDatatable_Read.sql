CREATE PROCEDURE [dbo].[usp_CustomerDatatable_Read]
	@PageSize INT,
	@PageSkip INT,
	@SearchBy NVARCHAR(50),
	@CustomerGroupId INT = 0,
	-- 0. Personal – Cá nhân
    -- 1. Organization – Tổ chức
	@Type SMALLINT = -1
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @PageSize INT, 
											  @PageSkip INT, 
											  @SearchBy NVARCHAR(50),
											  @CustomerGroupId INT,
											  @Type SMALLINT ',
				@ParamList2 NVARCHAR(2000) = '
											  @SearchBy NVARCHAR(50),
											  @CustomerGroupId INT,
											  @Type SMALLINT '
		
		SET @SqlStr = N'SELECT C.*, CG.[Name] AS [CustomerGroupName], CG.[PricingPolicy] AS [PricingPolicy], COUNT(*) OVER() AS [Count]
						FROM [dbo].[Customer] AS C
						INNER JOIN [dbo].[CustomerGroup] AS CG ON C.[CustomerGroupId] = CG.[Id]
						WHERE 1 = 1 '

		IF(@CustomerGroupId <> 0)
			SET @SqlStr = @SqlStr + N' AND C.[CustomerGroupId] = @CustomerGroupId '

		IF(@Type = 0)
			SET @SqlStr = @SqlStr + N' AND C.[Type] = 0 '
		IF(@Type = 1)
			SET @SqlStr = @SqlStr + N' AND C.[Type] = 1 '

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND C.[Name] LIKE ''%''+@SearchBy+''%'''

		IF(@PageSize > 0)
		BEGIN
			SET @SqlStr= @SqlStr + N' ORDER BY C.[Name] ASC 
									  OFFSET @PageSkip ROWS 
									  FETCH NEXT @PageSize ROWS ONLY '
			EXEC SP_EXECUTESQL @SqlStr, 
							   @ParamList,
							   @PageSize,
							   @PageSkip ,
							   @SearchBy,
							   @CustomerGroupId,
							   @Type
		END
		ELSE
		BEGIN
			SET @SqlStr= @SqlStr + N' ORDER BY C.[Name] ASC'
			EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList2,
						   @SearchBy,
						   @CustomerGroupId,
						   @Type
		END

		
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

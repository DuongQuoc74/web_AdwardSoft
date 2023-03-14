﻿CREATE PROCEDURE [dbo].[usp_ReceivingReportView_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE,
	@CreateUser VARCHAR(32),	
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
											  @CreateUser BIGINT,	
											  @SupplierId INT,
											  @BranchId INT '

		SET @SqlStr = N' SELECT R.[Id], R.[Date], R.[No], R.[TotalQuantity], R.[TotalAmount]
						, S.[Name] AS [Supplier], S.[Address], S.[Tel] AS [Phone], R.[Status] , S.[Id]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[ReceivingReport] R
						INNER JOIN [dbo].[Supplier] S ON S.[Id] = R.[SupplierId] 
						WHERE R.[BranchId] = @BranchId AND @DateFrom <=  CONVERT(DATE, R.[CreateDate]) AND  @DateTo >=  CONVERT(DATE, R.[CreateDate]) '


		IF(@SupplierId <> 0)
			SET @SqlStr =  @SqlStr + N' AND S.[Id] = @SupplierId '

		IF(@CreateUser <> 0)
			SET @SqlStr =  @SqlStr + N' AND R.[CreatedUser] = @CreateUser '

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND R.[No] LIKE ''%''+@SearchBy+''%'' OR S.[Name] LIKE ''%''+@SearchBy+''%'' '

		SET @SqlStr= @SqlStr + N' ORDER BY R.[CreateDate] DESC 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo,
						   @CreateUser,	
						   @SupplierId,
						   @BranchId

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


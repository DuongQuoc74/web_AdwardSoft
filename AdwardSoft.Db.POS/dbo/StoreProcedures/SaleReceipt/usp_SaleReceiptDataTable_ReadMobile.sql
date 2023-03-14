CREATE PROCEDURE [dbo].[usp_SaleReceiptDataTable_ReadMobile]
	@pageSize INT,
	@skip INT,
	@searchBy NVARCHAR(50),
	@UserId BIGINT,
	@DateFrom DATE,
	@DateTo DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr1 NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '	@pageSize INT,
												@skip INT,
												@searchBy NVARCHAR(50),
												@UserId BIGINT,
												@DateFrom DATE,
												@DateTo DATE '

		SET @SqlStr1 = N'SELECT sr.* , c.[Name] AS [CustomerName]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[SaleReceipt] sr
						INNER JOIN [dbo].[Customer] c ON c.[Id] = sr.[CustomerId]
						INNER Join [dbo].[EmployeeUser] E ON E.[UserId] = @UserId AND E.[EmployeeId] = SR.[EmployeeId]
						WHERE @DateFrom <=  CONVERT(DATE, sr.[Date]) AND  @DateTo >=  CONVERT(DATE, sr.[Date]) ';

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND (c.[Name] LIKE ''%''+@searchBy+''%'' OR sr.[No] LIKE ''%''+@searchBy+''%'') '
		END



		SET @SqlStr1 = @SqlStr1 +  N' ORDER BY sr.[Date] DESC, sr.[No]
									OFFSET @skip ROWS 
									FETCH NEXT @pageSize ROWS ONLY; '
		EXEC SP_EXECUTESQL @SqlStr1, 
						   @ParamList,
						   @pageSize,
						   @skip,
						   @searchBy,
						   @UserId,
						   @DateFrom,
						   @DateTo
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

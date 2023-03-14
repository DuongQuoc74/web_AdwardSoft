CREATE PROCEDURE [dbo].[usp_CustomerReport_Read]
	@pageSize INT,
	@pageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATE,
	@DateTo DATE
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
											  @DateFrom DATE,
											  @DateTo DATE'

		SET @SqlStr = N' SELECT c.[Name] AS [CustomerName]
						, c.[Id] AS [CustomerId]
						, c.[Phone] AS [CustomerPhone]
						, SUM(sr.[TotalAmount]) AS [TotalAmount]
						, SUM(earn.[Point]) AS [TotalPoint]
						, SUM(exchange.[Point]) AS [ExchangePoint]
						, ROUND(SUM(exchange.[Point])/20,0)*1000 AS [ExchangeAmount]
						, SUM(earn.[Point]) - SUM(exchange.[Point]) AS [BalancePoint]
						, (ROUND(SUM(earn.[Point])/20,0)*1000)- (ROUND(SUM(exchange.[Point])/20,0)*1000) AS [BalanceAmount]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[SaleReceipt] sr
						INNER JOIN [dbo].[Customer] c ON sr.[CustomerId] = c.[Id] 
						LEFT JOIN [dbo].[MembershipScore] exchange ON sr.[Id] = exchange.[SaleReceiptId] AND exchange.[Type] = 0 
						LEFT JOIN [dbo].[MembershipScore] earn ON sr.[Id] = earn.[SaleReceiptId] AND earn.[Type] = 1
						WHERE (sr.[Date] BETWEEN @DateFrom AND @DateTo) '

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND c.[Tag] LIKE ''%''+@SearchBy+''%'''

		SET @SqlStr= @SqlStr + N' GROUP BY c.[Id], c.[Name], c.[Phone]
								  ORDER BY SUM(sr.[TotalAmount]) DESC 
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @searchBy,
						   @DateFrom,
						   @DateTo

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
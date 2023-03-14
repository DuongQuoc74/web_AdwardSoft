CREATE PROCEDURE [dbo].[usp_CustomerClassDatatable_Read]
	@PageSize INT,
	@PageSkip INT,
	@SearchBy NVARCHAR(50),
	@DateFrom DATETIME,
	@DateTo DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @PageSize INT, 
											  @PageSkip INT, 
											  @SearchBy NVARCHAR(50),
											  @DateFrom DATETIME,
											  @DateTo DATETIME '

		SET @SqlStr = N' SELECT CC.[CustomerId], CC.[MembershipClassId], CC.[Date], CC.[OldMembershipClassId],
							C.[Name] AS [CustomerName], C.[Phone] AS [CustomerPhone], C.[Type] AS [CustomerType],
							CG.[Name] AS [CustomerGroupName], MC.[Name] AS [UpdateMembershipClassName], OMC.[Name] AS [OldMembershipClassName],
							COUNT(*) OVER() AS [Count]
						 FROM [dbo].[CustomerClass] AS CC
						 INNER JOIN [dbo].[Customer] AS C ON CC.[CustomerId] = C.[Id]
						 INNER JOIN [dbo].[CustomerGroup] AS CG ON C.[CustomerGroupId] = CG.[Id]
						 INNER JOIN [dbo].[MembershipClass] AS MC ON CC.[MembershipClassId] = MC.[Id]
						 INNER JOIN [dbo].[MembershipClass] AS OMC ON CC.[OldMembershipClassId] = OMC.[Id] 
						 WHERE @DateFrom <=  CONVERT(DATE, CC.[Date]) AND  @DateTo >=  CONVERT(DATE, CC.[Date])'

		IF(@SearchBy <> 'NULL')
			SET @SqlStr =  @SqlStr + N' AND C.[Name] LIKE ''%''+@SearchBy+''%'''

		SET @SqlStr= @SqlStr + N' ORDER BY CC.[Date] DESC 
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

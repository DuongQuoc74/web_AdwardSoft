CREATE PROCEDURE [dbo].[usp_EmployeeDataTable_Read]
	@PageSize INT,
	@PageSkip INT,
	@DepartmentId INT,
	@PositionId INT,
	@SearchBy NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@Count INT,
				@ParamList NVARCHAR(2000) = ' @pageSize INT, 
											  @PageSkip INT, 
											  @DepartmentId INT,
											  @PositionId INT,
											  @searchBy NVARCHAR(50) '

		SET @SqlStr = N' SELECT E.*, ES.*, U.[UserName]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[Employee] E
						LEFT JOIN [dbo].[EmployeeSalary] ES ON ES.[EmployeeId] = E.[Id] AND ES.[EffectiveDate] = ( SELECT TOP 1 ES.[EffectiveDate] FROM [dbo].[EmployeeSalary] AS ES WHERE ES.[EmployeeId] = E.[Id] ORDER BY ES.[EffectiveDate] DESC)
						LEFT JOIN [dbo].[EmployeeUser] EU ON EU.[EmployeeId] = E.[Id]
						LEFT JOIN [ADSHH.Identity].[dbo].[User] U ON U.[Id] = EU.[UserId]
						WHERE 1 = 1 ';
		
		IF(@DepartmentId <> 0)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND E.[DepartmentId] = @DepartmentId ';
		END

		IF(@PositionId <> 0)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND E.[PositionId] = @PositionId';
		END

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr = @SqlStr + N' AND E.[Name] LIKE ''%''+@SearchBy+''%'' OR E.[Code] LIKE ''%''+@SearchBy+''%''';
		END


		SET @SqlStr = @SqlStr + N' ORDER BY E.[Name] ASC 
								   OFFSET @PageSkip ROWS 
								   FETCH NEXT @pageSize ROWS ONLY; '

		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PageSize,
						   @PageSkip,
						   @DepartmentId,
						   @PositionId,
						   @searchBy						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

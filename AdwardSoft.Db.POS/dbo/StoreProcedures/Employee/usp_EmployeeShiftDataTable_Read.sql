CREATE PROCEDURE [dbo].[usp_EmployeeShiftDataTable_Read]
	@PageSize INT,
	@PageSkip INT,
	@EmployeeId INT,
	@BranchId INT,
	@ShiftId INT,
	@Date DATE,
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
											  @EmployeeId INT,
											  @BranchId INT,
											  @ShiftId INT,
											  @Date DATE,
											  @searchBy NVARCHAR(50) '

		SET @SqlStr = N' SELECT ES.*, S.[Name] AS [ShiftName], E.[Name] AS [EmployeeName], E.[Code] AS [EmployeeCode]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[EmployeeShift] ES
						INNER JOIN [dbo].[Shift] S ON ES.[ShiftId] = S.[Id]
						INNER JOIN [dbo].[Employee] E ON ES.[EmployeeId] = E.[Id]
						WHERE S.[BranchId] = @BranchId AND ES.[Year] = YEAR(@Date) AND ES.[Month] = MONTH(@Date) ';
		
		IF(DATEPART(dw,@Date) = 2)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsMonday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 3)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsTuesday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 4)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsWednesday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 5)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsThursday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 6)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsFriday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 7)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsSaturday] = 1 ';
		END

		IF(DATEPART(dw,@Date) = 8)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[IsSunday] = 1 ';
		END

		IF(@EmployeeId <> 0)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND E.[Id] = @EmployeeId ';
		END		

		IF(@ShiftId <> 0)
		BEGIN
			SET @SqlStr = @SqlStr + N' AND S.[Id] = @ShiftId ';
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
						   @EmployeeId,
						   @BranchId,
						   @ShiftId,
						   @Date,
						   @searchBy						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


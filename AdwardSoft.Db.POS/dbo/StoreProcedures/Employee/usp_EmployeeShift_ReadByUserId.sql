CREATE PROCEDURE [dbo].[usp_EmployeeShift_ReadByUserId]
	@EmployeeId INT,
	@BranchId INT,
	@Date DATE
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@Count INT,
				@ParamList NVARCHAR(2000) = ' @EmployeeId INT,
											  @BranchId INT,									
											  @Date DATE '

		SET @SqlStr = N' SELECT ES.*, S.[Name] AS [ShiftName], E.[Name] AS [EmployeeName], E.[Code] AS [EmployeeCode]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[EmployeeShift] ES
						INNER JOIN [dbo].[Shift] S ON ES.[ShiftId] = S.[Id]
						INNER JOIN [dbo].[Employee] E ON ES.[EmployeeId] = E.[Id]
						WHERE S.[BranchId] = @BranchId AND ES.[Year] = YEAR(@Date) AND ES.[Month] = MONTH(@Date) AND E.[Id] = @EmployeeId';
		
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

	
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @EmployeeId,
						   @BranchId,
						   @Date						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
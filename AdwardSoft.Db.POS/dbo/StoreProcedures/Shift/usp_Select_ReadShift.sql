CREATE PROCEDURE [dbo].[usp_Select_ReadShift]
	@UserId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		IF (@UserId <> 0)
		BEGIN
			SELECT s.[Id], s.[Name] AS [Text]
			FROM [Shift] s
			INNER JOIN [dbo].[EmployeeShift] es ON es.[ShiftId] = s.[Id]
			WHERE es.[EmployeeId] = @UserId AND YEAR(GETDATE()) = CONVERT(INT, es.[Year]) AND MONTH(GETDATE()) = CONVERT(INT, es.[Month])
		END
		ELSE
		BEGIN
			SELECT s.[Id], s.[Name] AS [Text]
			FROM [Shift] s
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

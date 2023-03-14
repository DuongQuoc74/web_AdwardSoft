CREATE PROCEDURE [dbo].[usp_EmployeeShift_ReadIsExist]
	@EmployeeId INT, 
    @ShiftId INT,
    @Month NUMERIC(2, 0), 
    @Year NUMERIC(4,0)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF NOT EXISTS ( SELECT TOP 1 1 FROM [dbo].[EmployeeShift] p
						WHERE p.[EmployeeId] = @EmployeeId 
						AND p.[ShiftId] = @ShiftId 
						AND p.[Month] = @Month 
						AND p.[Year] = @Year)
				SELECT 0
			ELSE 
				SELECT 1
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

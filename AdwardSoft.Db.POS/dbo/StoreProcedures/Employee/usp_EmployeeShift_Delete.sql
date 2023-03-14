CREATE PROCEDURE [dbo].[usp_EmployeeShift_Delete]
	@EmployeeId INT, 
    @ShiftId INT,
    @Month NUMERIC(2, 0), 
    @Year NUMERIC(4,0)
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[EmployeeCheckoutCounter]
			WHERE [EmployeeId] = @EmployeeId AND  [ShiftId] = @ShiftId  AND  [Month] = @Month AND  [Year] = @Year

			DELETE [dbo].[EmployeeShift]
			WHERE [EmployeeId] = @EmployeeId AND  [ShiftId] = @ShiftId AND  [Month] = @Month AND  [Year] = @Year
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

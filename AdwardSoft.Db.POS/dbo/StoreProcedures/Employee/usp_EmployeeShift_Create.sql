CREATE PROCEDURE [dbo].[usp_EmployeeShift_Create]
	@EmployeeId INT, 
    @ShiftId INT, 
    @Month NUMERIC(2, 0), 
    @Year NUMERIC(4,0),
    @Type TINYINT,
    @CheckoutCounterId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[EmployeeShift] ([EmployeeId],[ShiftId],[Month],[Year],[Type])
			VALUES(@EmployeeId, @ShiftId, @Month, @Year, @Type)

			IF (@Type = 1)
			BEGIN
				INSERT INTO [dbo].[EmployeeCheckoutCounter] ([EmployeeId],[ShiftId],[Month],[Year],[CheckoutCounterId])
				VALUES(@EmployeeId, @ShiftId, @Month, @Year, @CheckoutCounterId)
			END
		COMMIT
		SELECT 1
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		RETURN 0;
		THROW;
	END CATCH
END

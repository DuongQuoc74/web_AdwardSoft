CREATE PROCEDURE [dbo].[usp_EmployeeShift_ReadById]
	@EmployeeId INT, 
    @ShiftId INT,
    @Month NUMERIC(2, 0), 
    @Year NUMERIC(4,0)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT ES.*, ISNULL(ECC.[CheckoutCounterId], 0) AS [CheckoutCounterId]
		FROM [dbo].[EmployeeShift] ES
		LEFT JOIN [dbo].[EmployeeCheckoutCounter] ECC ON ES.[EmployeeId] = ECC.[EmployeeId] AND ES.[ShiftId] = ECC.[ShiftId] AND ES.[Month] = ECC.[Month] AND ES.[Year] = ECC.[Year]
		WHERE ES.[EmployeeId] = @EmployeeId AND ES.[ShiftId] = @ShiftId  AND  ES.[Month] = @Month AND  ES.[Year] = @Year
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

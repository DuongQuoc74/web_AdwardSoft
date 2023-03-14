CREATE PROCEDURE [dbo].[usp_Timesheet_Read]
	@UserId BIGINT,
	@Month NUMERIC(2,0),
	@Year NUMERIC(4,0)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[Timesheet] p
		WHERE p.[UserId] = @UserId AND MONTH([Date]) = @Month AND YEAR([Date]) = @Year
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

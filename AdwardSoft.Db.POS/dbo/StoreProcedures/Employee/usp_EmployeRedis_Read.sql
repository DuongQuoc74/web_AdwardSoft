CREATE PROCEDURE [dbo].[usp_EmployeRedis_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[Employee]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


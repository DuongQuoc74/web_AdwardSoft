CREATE PROCEDURE [dbo].[usp_Department_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT d.* 
		FROM [Department] d
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

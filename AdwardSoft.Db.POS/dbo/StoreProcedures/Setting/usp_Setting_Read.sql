CREATE PROCEDURE [dbo].[usp_Setting_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[Setting] p	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

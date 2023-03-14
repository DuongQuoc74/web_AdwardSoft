CREATE PROCEDURE [dbo].[usp_HandleError_Read]
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[HandleError] h
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_PriceList_ReadDateIsExisting]
	@Date DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[PriceList] AS P
		WHERE P.[Date] = @Date

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
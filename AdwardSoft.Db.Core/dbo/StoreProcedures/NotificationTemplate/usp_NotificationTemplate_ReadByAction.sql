CREATE PROCEDURE [dbo].[usp_NotificationTemplate_ReadByAction]
	@Action TINYINT
AS 
	BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[NotificationTemplate]
		WHERE [Action] = @Action
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

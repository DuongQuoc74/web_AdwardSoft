CREATE PROCEDURE [dbo].[usp_NotificationTemplate_Delete]
		@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [NotificationTemplate]
			WHERE [Id] = @Id
		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		SELECT 0;
		ROLLBACK;
		THROW;
	END CATCH
END
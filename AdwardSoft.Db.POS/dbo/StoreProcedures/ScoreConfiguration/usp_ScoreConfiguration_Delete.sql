CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DECLARE @count INT = (SELECT COUNT(*) FROM [dbo].[ScoreConfiguration] WHERE [Id] <> @Id)
			IF(@count != 0)
			BEGIN
				DELETE FROM [dbo].[ScoreConfiguration]
				WHERE [Id] = @Id
			END
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

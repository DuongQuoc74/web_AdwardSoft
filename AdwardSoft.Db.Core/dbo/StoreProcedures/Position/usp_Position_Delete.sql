CREATE PROCEDURE [dbo].[usp_Position_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[PositionTrans]
			WHERE [PositionId] = @Id

			DELETE FROM [dbo].[Position]
			WHERE [Id] = @Id
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0
		THROW;
	END CATCH
END
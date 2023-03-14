CREATE PROCEDURE [dbo].[usp_HandleError_Delete]
	@Id INT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return INT = 1
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[HandleError]
			WHERE [Id] = @Id

			DELETE FROM [dbo].[HandleErrorTrans]
			WHERE [HandleErrorId] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0;
		THROW;
	END CATCH
	SELECT @return
END
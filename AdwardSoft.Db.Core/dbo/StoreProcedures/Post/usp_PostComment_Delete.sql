CREATE PROCEDURE [dbo].[usp_PostComment_Delete]
	@Id CHAR(32)
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			DELETE [dbo].[PostComment]
			WHERE [ParentId] = @Id

			DELETE [dbo].[PostComment]
			WHERE [Id] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0;
		THROW;
	END CATCH
	SELECT @return
END


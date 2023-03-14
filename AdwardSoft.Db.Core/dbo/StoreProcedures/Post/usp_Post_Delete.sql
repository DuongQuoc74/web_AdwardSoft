CREATE PROCEDURE [dbo].[usp_Post_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return INT = 1
	BEGIN TRY
		BEGIN TRAN
			DELETE [PostSEO]
			WHERE [PostId] = @Id

			DELETE [PostGallery]
			WHERE [PostId] = @Id

			DELETE [Post]
			WHERE [Id] = @Id
		COMMIT
		--SELECT 1;
	END TRY
	BEGIN CATCH
		SET @return = 0;
		THROW;
	END CATCH
	SELECT @return;
	RETURN @return;
END
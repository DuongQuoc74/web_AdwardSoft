CREATE PROCEDURE [dbo].[usp_PostGallery_Delete]
	@Id CHAR(32)
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	------------------------------------------------
	DECLARE @Return BIT = 1
	BEGIN TRY
		DELETE [dbo].[PostGallery]
		WHERE [Id] = @Id
	END TRY
	BEGIN CATCH
		SET @Return = 0
		ROLLBACK;
		THROW
	END CATCH
	SELECT @Return
END

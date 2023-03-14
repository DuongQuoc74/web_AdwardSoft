CREATE PROCEDURE [dbo].[usp_PostGallery_Create]
	@Id CHAR(32),
	@PostId INT,
	@Image	VARCHAR(2048)
AS
BEGIN 
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @Return CHAR(32)
	BEGIN TRY
		BEGIN TRAN
			SET @Id = REPLACE(NEWID(), '-', '')

			INSERT INTO [dbo].[PostGallery] ([Id], [PostId], [Image])
			VALUES(@Id, @PostId, @Image)
		COMMIT
	END TRY
	BEGIN CATCH
		SET @Return = '0'
		ROLLBACK;
		THROW
	END CATCH
	SELECT @Id
END

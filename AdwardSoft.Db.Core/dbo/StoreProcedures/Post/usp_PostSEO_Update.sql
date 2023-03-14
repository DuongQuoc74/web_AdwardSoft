CREATE PROCEDURE [dbo].[usp_PostSEO_Update]
	@PostId INT , 
	@Title NVARCHAR(71) , 
    @Description NVARCHAR(160), 
	@CanonicalURL VARCHAR(2048) , 
	@MetaRobotIndex    TINYINT , --1: Index, 0: NoIndex
    @MetaRobotFollow   TINYINT , --1: Follow, 0: NoFollow
    @MetaRobotAdvanced TINYINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [PostSEO]
			SET [Title] = @Title,
				[Description] = @Description,
				[CanonicalURL] = @CanonicalURL,
				[MetaRobotIndex] = @MetaRobotIndex,
				[MetaRobotFollow] = @MetaRobotFollow,
				[MetaRobotAdvanced] = @MetaRobotAdvanced
			WHERE [PostId] = @PostId
		COMMIT
		--SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		--SELECT 0;
		THROW;
	END CATCH
END

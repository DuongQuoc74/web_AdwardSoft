CREATE PROCEDURE [dbo].[usp_PostComment_Update]
	@Id CHAR(32),
	@PostId BIGINT,
	@UserId BIGINT,
	@Date SMALLDATETIME,
	@ParentId CHAR(32),
	@Comment NVARCHAR(300),
	@Status TINYINT,
	@UserName NVARCHAR(300),
	@Avatar NVARCHAR(300)  
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[PostComment]
			SET [Comment] = @Comment
			WHERE [Id] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

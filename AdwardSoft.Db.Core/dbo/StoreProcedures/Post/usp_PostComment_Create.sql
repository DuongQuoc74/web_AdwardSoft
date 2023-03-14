CREATE PROCEDURE [dbo].[usp_PostComment_Create]
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
	BEGIN TRY
		BEGIN TRAN;
			SET @Id = REPLACE(NEWID(), '-','');
			INSERT [dbo].[PostComment] ([Id], [PostId], [UserId], [Date], [ParentId], [Comment], [Status])
			VALUES (@Id, @PostId, @UserId, @Date, @ParentId, @Comment, @Status)
		COMMIT	
	END TRY
	BEGIN CATCH
		SELECT '' AS [Id]
		ROLLBACK TRAN
		THROW;
	END CATCH
	SELECT @Id AS [Id]
END

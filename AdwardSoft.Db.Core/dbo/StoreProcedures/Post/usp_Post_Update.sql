CREATE PROCEDURE [dbo].[usp_Post_Update]
	@Id INT , 
    @Status TINYINT, --0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
    @Visibility TINYINT , --0: Public, 1: Password Protected, 2: Private
    @IsStick BIT  , -- Only visible > Visibility = Public
    @PasswordProtected VARCHAR(60) , -- Only visible > Visibility = Password Protected
    @PublishedOn SMALLDATETIME  , 
    @Format TINYINT  , --0: Standard, 1: Image, 2: Video, 3: Quote, 4: Gallery
    @FeaturedImage VARCHAR(2048)  , 
	@Title NVARCHAR(150)  , 
    @Description NVARCHAR(300) , 
    @Content NVARCHAR(MAX)  , 
    @Permalink VARCHAR(512)  , 
    @IsMenuLink BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [Post]
			SET [Status] = @Status,
				[Visibility] = @Visibility,
				[IsStick] = @IsStick,
				[PasswordProtected] = @PasswordProtected,
				[PublishedOn] = @PublishedOn,
				[Format] = @Format,
				[FeaturedImage] = @FeaturedImage,
				[Title] = @Title,
				[Description] = @Description,
				[Content] = @Content,
				[Permalink] = @Permalink,
				[IsMenuLink] = @IsMenuLink
			WHERE [Id] = @Id
		COMMIT
		--SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		--SELECT 0;
		THROW;
	END CATCH
END

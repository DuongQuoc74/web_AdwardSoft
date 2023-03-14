CREATE PROCEDURE [dbo].[usp_PostGallery_ReadByPostId]
	@PostId INT
AS
BEGIN
	SELECT * FROM [dbo].[PostGallery] WHERE [PostId] = @PostId
END
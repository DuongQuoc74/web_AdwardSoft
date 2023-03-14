CREATE PROCEDURE [dbo].[usp_Post_ReadPagination]
	@pageIndex int,
	@pageSize int
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	DECLARE @NewId INT
	
	BEGIN TRY
		BEGIN TRAN
			SELECT tmp.*, pg.[Image]
			FROM ( 
					SELECT *, COUNT(*) OVER() AS 'Count'
					FROM Post p
					where p.[Status] = 2
					ORDER BY p.[PublishedOn] DESC, p.[Id]
					OFFSET (@pageSize * @pageIndex) ROWS 
					FETCH NEXT @pageSize ROWS ONLY 
				 ) tmp
			LEFT JOIN [dbo].[PostGallery] pg ON pg.[PostId] = tmp.[Id]
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END
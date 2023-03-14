CREATE PROCEDURE [dbo].[usp_DataTablePostInfor_ReadInfoBasic]
	@skip INT,
	@pageSize INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT [Id],[Title],[Description],[FeaturedImage]
		FROM [dbo].[Post]
		ORDER BY [PublishedOn] DESC
		OFFSET @skip ROWS
		FETCH NEXT @pageSize ROWS ONLY;	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
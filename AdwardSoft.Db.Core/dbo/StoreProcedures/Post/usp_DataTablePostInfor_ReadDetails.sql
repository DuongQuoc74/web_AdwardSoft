CREATE PROCEDURE [dbo].[usp_DataTablePostInfor_ReadDetails]
	@skip INT,
	@pageSize INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[Post]
		ORDER BY [PublishedOn] DESC
		OFFSET @skip ROWS
		FETCH NEXT @pageSize ROWS ONLY;	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

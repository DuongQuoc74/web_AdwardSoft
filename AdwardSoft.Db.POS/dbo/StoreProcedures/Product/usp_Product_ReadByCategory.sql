CREATE PROCEDURE [dbo].[usp_Product_ReadByCategory]
	@CategoryId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT P.*
		FROM [dbo].[Product] AS P
		INNER JOIN [dbo].[ProductCategory] PC ON P.[Id] = PC.[CategoryId]
		WHERE PC.[CategoryId] = @CategoryId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

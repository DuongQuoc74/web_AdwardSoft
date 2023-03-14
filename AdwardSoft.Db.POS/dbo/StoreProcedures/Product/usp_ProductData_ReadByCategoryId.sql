CREATE PROCEDURE [dbo].[usp_ProductData_ReadByCategoryId]
	@CategoryId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT P.*,C.[Id] AS CategoryId, C.[Name] AS CategoryName
		FROM [dbo].[Product] AS P, [dbo].[ProductCategory] AS PC,[dbo].[Category] AS C
		WHERE P.[Id] = PC.[ProductId] AND PC.[CategoryId] = C.[Id]
		AND PC.[CategoryId]=@CategoryId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

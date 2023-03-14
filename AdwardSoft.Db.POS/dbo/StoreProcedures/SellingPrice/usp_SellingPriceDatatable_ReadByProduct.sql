CREATE PROCEDURE [dbo].[usp_SellingPriceDatatable_ReadByProduct]
	@ProductId INT,
	@pageSize INT,
	@pageSkip INT,
	@UnitId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
			SELECT b.*,  u.[Name] AS [UnitName] ,COUNT(*) OVER() AS [Count]
			FROM [dbo].[SellingPrice] b
			INNER JOIN [dbo].[Unit] u ON u.[Id] = b.[UnitId]
			WHERE B.[ProductId] = @ProductId AND (@UnitId = 0 OR [UnitId] = @UnitId)
			ORDER BY b.[ProductId], b.[Date] DESC
			OFFSET @pageSkip ROWS 
			FETCH NEXT @pageSize ROWS ONLY		   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

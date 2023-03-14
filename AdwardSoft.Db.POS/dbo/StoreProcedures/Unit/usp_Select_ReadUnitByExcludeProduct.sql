CREATE PROCEDURE [dbo].[usp_Select_ReadUnitByExcludeProduct]
	@ProductId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT u.[Id], u.[Name] AS [Text]
		FROM [dbo].[Unit] u	
		WHERE u.[Status] = 1  AND u.[Id] <> (SELECT [UnitId] FROM [dbo].[Product] p WHERE  p.[Id] = @ProductId) 			
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

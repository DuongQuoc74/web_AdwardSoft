CREATE PROCEDURE [dbo].[usp_BranchStock_ReadByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT b.* , s.[Id] AS [StockId], s.[Name] AS [StockName]
		FROM [Branch] b
		INNER JOIN [UserBranch] ub ON ub.[BranchId] = b.[Id]
		INNER JOIN [Stock] s ON s.[Id] = (SELECT TOP 1 [Id] FROM [Stock] WHERE [BranchId] = B.[Id] ORDER BY [IsDefault] DESC)
		WHERE ub.[UserId] = @UserId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

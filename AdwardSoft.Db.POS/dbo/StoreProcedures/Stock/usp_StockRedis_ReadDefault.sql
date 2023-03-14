CREATE PROCEDURE [dbo].[usp_StockRedis_ReadDefault]
	@BranchId INT
AS
	SELECT S.*
	FROM [dbo].[Stock] AS S
	WHERE S.[BranchId] = @BranchId AND S.[IsDefault] = 1

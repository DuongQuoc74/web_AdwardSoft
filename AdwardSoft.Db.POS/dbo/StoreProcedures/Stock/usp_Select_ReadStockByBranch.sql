CREATE PROCEDURE [dbo].[usp_Select_ReadStockByBranch]
	@BranchId INT
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Stock]
	WHERE [BranchId] = @BranchId
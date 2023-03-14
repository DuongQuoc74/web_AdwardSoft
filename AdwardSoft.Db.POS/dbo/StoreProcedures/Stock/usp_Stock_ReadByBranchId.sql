CREATE PROCEDURE [dbo].[usp_Stock_ReadByBranchId]
	@BranchId INT
AS
	SELECT *
	FROM [dbo].[Stock]
	WHERE [BranchId] = @BranchId

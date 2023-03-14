CREATE PROCEDURE [dbo].[usp_Stock_ReadDefault]
	@BranchId INT = 0
AS
	IF(@BranchId <> 0)
		SELECT S.*
		FROM [dbo].[Inventory] AS S
		WHERE S.[BranchId] = @BranchId AND S.[IsDefault] = 1

	ELSE
		SELECT S.*
		FROM [dbo].[Inventory] AS S
		WHERE S.[IsDefault] = 1

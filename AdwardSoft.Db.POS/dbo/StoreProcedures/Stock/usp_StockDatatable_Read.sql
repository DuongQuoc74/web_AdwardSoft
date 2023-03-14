CREATE PROCEDURE [dbo].[usp_StockDatatable_Read]
	@BranchId INT
AS
	SELECT * 
	FROM [dbo].[Stock]
	WHERE [BranchId] = @BranchId
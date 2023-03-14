CREATE PROCEDURE [dbo].[usp_CheckoutCounterDatatable_Read]
	@BranchId INT
AS
	SELECT *
	FROM [dbo].[CheckoutCounter]
	WHERE [BranchId] = @BranchId

CREATE PROCEDURE [dbo].[usp_ShiftDatatable_Read]
	@BranchId INT
AS
	SELECT *
	FROM [dbo].[Shift]
	WHERE [BranchId] = @BranchId

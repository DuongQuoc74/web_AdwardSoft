CREATE PROCEDURE [dbo].[usp_SupplierPOS_Read]
AS
	SELECT t.[Idx], t.[Name] AS [ContactName], t.[Note], t.[Phone], t.[Position], d.*
	FROM [dbo].[SupplierContact] t INNER JOIN 
	[dbo].[Supplier] d ON t.SupplierId = d.Id
CREATE PROCEDURE [dbo].[usp_Supplier_ReadDefault]
AS
	SELECT TOP 1 *
	FROM [dbo].[Supplier]
	WHERE [IsDefault] = 1

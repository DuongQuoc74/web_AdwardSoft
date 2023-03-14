CREATE PROCEDURE [dbo].[usp_Supplier_ReadById]
	@Id INT
AS
	SELECT * 
	FROM [dbo].[Supplier]
	WHERE [Id] = @Id
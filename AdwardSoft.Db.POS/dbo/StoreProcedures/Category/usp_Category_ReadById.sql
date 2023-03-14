CREATE PROCEDURE [dbo].[usp_Category_ReadById]
	@Id INT
AS
	SELECT * 
	FROM [dbo].[Category]
	WHERE [Id] = @Id
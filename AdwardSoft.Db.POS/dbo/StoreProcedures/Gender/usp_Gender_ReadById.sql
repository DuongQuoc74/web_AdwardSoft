CREATE PROCEDURE [dbo].[usp_Gender_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[Gender]
	WHERE [Id] = @Id

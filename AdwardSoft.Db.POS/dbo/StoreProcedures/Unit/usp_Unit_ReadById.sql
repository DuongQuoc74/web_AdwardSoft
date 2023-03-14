CREATE PROCEDURE [dbo].[usp_Unit_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[Unit]
	WHERE [Id] = @Id
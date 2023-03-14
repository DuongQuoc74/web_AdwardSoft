CREATE PROCEDURE [dbo].[usp_Position_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[Position]
	WHERE [Id] = @Id


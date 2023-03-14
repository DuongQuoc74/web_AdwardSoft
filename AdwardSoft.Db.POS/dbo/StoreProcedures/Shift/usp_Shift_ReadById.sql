CREATE PROCEDURE [dbo].[usp_Shift_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[Shift]
	WHERE [Id] = @Id
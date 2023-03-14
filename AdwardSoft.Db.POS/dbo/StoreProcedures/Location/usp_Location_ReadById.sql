CREATE PROCEDURE [dbo].[usp_Location_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[Location]
	WHERE [Id] = @Id


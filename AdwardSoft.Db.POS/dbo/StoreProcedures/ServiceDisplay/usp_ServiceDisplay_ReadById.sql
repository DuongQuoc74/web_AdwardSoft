CREATE PROCEDURE [dbo].[usp_ServiceDisplay_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[ServiceDisplay]
	WHERE [Id] = @Id

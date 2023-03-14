CREATE PROCEDURE [dbo].[usp_Contact_ReadById]
	@Id INT
AS
BEGIN
	SELECT *
	FROM [Contact]
	WHERE [Id] = @Id
END
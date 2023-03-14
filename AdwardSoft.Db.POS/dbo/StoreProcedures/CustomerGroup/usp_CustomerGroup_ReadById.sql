CREATE PROCEDURE [dbo].[usp_CustomerGroup_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[CustomerGroup]
	WHERE [Id] = @Id

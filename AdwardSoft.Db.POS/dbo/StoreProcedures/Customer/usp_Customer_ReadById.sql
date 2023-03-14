CREATE PROCEDURE [dbo].[usp_Customer_ReadById]
	@Id INT
AS
	SELECT * 
	FROM [dbo].[Customer]
	WHERE [Id] = @Id
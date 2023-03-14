CREATE PROCEDURE [dbo].[usp_CheckoutCounter_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[CheckoutCounter]
	WHERE [Id] = @Id
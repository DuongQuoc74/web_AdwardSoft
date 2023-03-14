CREATE PROCEDURE [dbo].[usp_PaymentMethod_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[PaymentMethod]
	WHERE [Id] = @Id


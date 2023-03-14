CREATE PROCEDURE [dbo].[usp_BankAccount_ReadById]
	@Id INT
AS 
SELECT *
FROM [dbo].[BankAccount]
WHERE [Id] = @Id
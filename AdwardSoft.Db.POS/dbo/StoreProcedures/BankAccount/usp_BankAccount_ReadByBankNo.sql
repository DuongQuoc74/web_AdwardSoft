CREATE PROCEDURE [dbo].[usp_BankAccount_ReadByBankNo]
	@Id INT,
	@BankNo VARCHAR(20)
AS 
SELECT *
FROM [dbo].[BankAccount]
WHERE [BankNo] = @BankNo AND [Id] <> @Id
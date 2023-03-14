CREATE PROCEDURE [dbo].[usp_Select_ReadBankAccount]
AS
SELECT [Id], [BankNo] + ' - ' + [BankName] as [Text]
FROM [dbo].[BankAccount]
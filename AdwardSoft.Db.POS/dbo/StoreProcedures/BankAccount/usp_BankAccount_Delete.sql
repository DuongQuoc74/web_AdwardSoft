CREATE PROCEDURE [dbo].[usp_BankAccount_Delete]
	@Id INT
AS 
BEGIN 
	SET NOCOUNT OFF
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

	BEGIN TRY
	
		BEGIN TRAN	
		
			DECLARE @Check INT = 0 

			SELECT TOP 1 @Check = 1 
			FROM [dbo].[CustomerWallet] 
			WHERE [BankAccountId] = @Id

			IF @Check = 0
			BEGIN
				DELETE FROM [dbo].[BankAccount] WHERE [Id] = @Id 
				SELECT 1;
			END
			ELSE 
				SELECT 0;

		COMMIT
	END TRY 
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH 
END
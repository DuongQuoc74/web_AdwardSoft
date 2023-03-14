CREATE PROCEDURE [dbo].[usp_BankAccount_Create]
	@Id INT,
	@BankNo VARCHAR (20),
	@BankName NVARCHAR(120),
	@Status TINYINT
AS 
BEGIN 
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
        BEGIN TRAN
			INSERT INTO [dbo].[BankAccount]([BankNo],[BankName],[Status]) VALUES (@BankNo, @BankName, @Status)
			SET @Id = SCOPE_IDENTITY()
		COMMIT
		SELECT B.*
		FROM [dbo].[BankAccount] AS B
		WHERE B.[Id] = @Id
	END TRY 
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW
	END CATCH
END


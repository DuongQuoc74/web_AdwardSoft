CREATE PROCEDURE [dbo].[usp_BankAccount_Update]
    @Id INT,
    @BankNo VARCHAR(20),
    @BankName NVARCHAR(120),
    @Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
    BEGIN TRY
        BEGIN TRAN
            UPDATE [dbo].[BankAccount]
            SET [BankNo] = @BankNo, 
                [BankName] = @BankName,                
               [Status] = @Status
            WHERE [Id] = @Id
        COMMIT
        SELECT B.*
        FROM [dbo].[BankAccount] AS B
        WHERE B.[Id] = @Id
    END TRY
    BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
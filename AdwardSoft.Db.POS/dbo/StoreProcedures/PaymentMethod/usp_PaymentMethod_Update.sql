CREATE PROCEDURE [dbo].[usp_PaymentMethod_Update]
	@Id INT,
    @Name NVARCHAR(32),
    @Icon NVARCHAR(2048)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[PaymentMethod]
			SET [Name] = @Name,
				[Icon] = @Icon
			WHERE [Id] = @Id
		COMMIT
		SELECT P.* 
		FROM [dbo].[PaymentMethod] AS P
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
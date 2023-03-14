CREATE PROCEDURE [dbo].[usp_PaymentMethod_Create]
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
			INSERT INTO [dbo].[PaymentMethod]
					([Name], [Icon])
			VALUES  (@Name, @Icon)
			SET @Id = SCOPE_IDENTITY()
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
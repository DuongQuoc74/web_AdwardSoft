CREATE PROCEDURE [dbo].[usp_CheckoutCounter_Update]
	@Id INT, 
    @Name NVARCHAR(120), 
    @Status TINYINT, 
    @BranchId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[CheckoutCounter]
			SET [Name] = @Name, 
				[Status] = @Status
			WHERE [Id] = @Id

		COMMIT

		SELECT *  
		FROM [dbo].[CheckoutCounter]
		WHERE [Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
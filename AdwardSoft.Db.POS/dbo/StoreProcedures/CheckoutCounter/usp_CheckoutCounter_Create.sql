CREATE PROCEDURE [dbo].[usp_CheckoutCounter_Create]
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
			INSERT INTO [dbo].[CheckoutCounter]
					([Name], [Status], [BranchId])
			VALUES  (@Name , @Status , @BranchId)

			SET @Id = SCOPE_IDENTITY()

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
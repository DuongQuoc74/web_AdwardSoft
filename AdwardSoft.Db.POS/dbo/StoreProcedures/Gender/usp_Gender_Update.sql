CREATE PROCEDURE [dbo].[usp_Gender_Update]
	@Id INT,
    @Name NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Gender]
			SET [Name] = @Name
			WHERE [Id] = @Id
		COMMIT
		SELECT G.* 
		FROM [dbo].[Gender] AS G
		WHERE G.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

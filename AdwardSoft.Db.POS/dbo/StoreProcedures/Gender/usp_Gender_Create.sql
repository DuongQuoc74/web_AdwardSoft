CREATE PROCEDURE [dbo].[usp_Gender_Create]
	@Id INT,
    @Name NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[Gender]
					([Name])
			VALUES  (@Name)
			SET @Id = SCOPE_IDENTITY()
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

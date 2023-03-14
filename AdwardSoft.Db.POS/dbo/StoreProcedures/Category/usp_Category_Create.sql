CREATE PROCEDURE [dbo].[usp_Category_Create]
	@Id INT, 
    @Name NVARCHAR(60), 
    @Description NVARCHAR(100),
    @Status INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[Category] ([Name],[Description],[Status])
			VALUES(@Name, @Description, @Status)

			SET @Id = SCOPE_IDENTITY()
		COMMIT

		SELECT S.* 
		FROM [dbo].[Category] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

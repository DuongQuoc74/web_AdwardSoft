CREATE PROCEDURE [dbo].[usp_Position_Create]
	@Id INT,
    @Name NVARCHAR(80),
    @Description NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[Position]
					([Name], [Description])
			VALUES  (@Name, @Description)
			SET @Id = SCOPE_IDENTITY()
		COMMIT
		SELECT P.* 
		FROM [dbo].[Position] AS P
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END


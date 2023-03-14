CREATE PROCEDURE [dbo].[usp_Position_Update]
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
			UPDATE [dbo].[Position]
			SET [Name] = @Name,
				[Description] = @Description
			WHERE [Id] = @Id
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

CREATE PROCEDURE [dbo].[usp_Department_Update]
	@Id INT,
	@Name NVARCHAR(120),
	@Description NVARCHAR(128),
	@ParentId INT,
	@Sort INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Department]
			SET [Name] = @Name,
				[Description] = @Description
			WHERE [Id] = @Id
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

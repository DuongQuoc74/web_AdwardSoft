CREATE PROCEDURE [dbo].[usp_Branch_Update]
	@Id INT,
	@Name NVARCHAR(120),
	@Address NVARCHAR(128),
	@Tel VARCHAR(12),
	@Email VARCHAR(254),
	@ParentId INT,
	@Sort INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Branch]
			SET [Name] = @Name,
				[Address] = @Address,
				[Tel] = @Tel,
				[Email] = @Email
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

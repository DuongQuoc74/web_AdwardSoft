CREATE PROCEDURE [dbo].[usp_Supplier_Update]
	@Id INT, 
    @Name NVARCHAR(120), 
    @Address NVARCHAR(128), 
    @Tel VARCHAR(12), 
    @Email VARCHAR(254),
	@IsDefault BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF (@IsDefault = 1)
			BEGIN
				UPDATE [dbo].[Supplier]
				SET [IsDefault] = 0
			END

			UPDATE [dbo].[Supplier]
			SET [Name] = @Name,
				[Address] = @Address,
				[Tel] = @Tel,
				[Email] = @Email
			WHERE [Id] = @Id
		COMMIT
		SELECT S.* 
		FROM [dbo].[Supplier] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

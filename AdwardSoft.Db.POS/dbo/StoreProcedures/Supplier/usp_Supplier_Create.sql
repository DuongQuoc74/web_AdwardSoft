CREATE PROCEDURE [dbo].[usp_Supplier_Create]
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
				INSERT INTO [dbo].[Supplier] ([Name],[Address],[Tel],[Email], [IsDefault])
				VALUES(@Name, @Address, @Tel, @Email, @IsDefault)

				SET @Id = SCOPE_IDENTITY()
		COMMIT
		SELECT S.* 
		FROM [dbo].[Supplier] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END
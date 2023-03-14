CREATE PROCEDURE [dbo].[usp_Category_Update]
	@Id INT, 
    @Name NVARCHAR(60), 
    @Description NVARCHAR(100), 
    @Status int
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Category]
			SET [Name] = @Name,
				[Description] = @Description,
				[Status] = @Status
			WHERE [Id] = @Id
		COMMIT

		SELECT S.* 
		FROM [dbo].[Category] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

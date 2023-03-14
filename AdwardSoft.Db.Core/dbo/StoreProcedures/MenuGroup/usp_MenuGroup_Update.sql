CREATE PROCEDURE [dbo].[usp_MenuGroup_Update]
	@Id INT,
	@Name NVARCHAR(150),
	@Position TINYINT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[MenuGroup]
			SET [Name] = @Name,
				[Position] = @Position
			WHERE [Id] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

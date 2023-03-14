CREATE PROCEDURE [dbo].[usp_Stock_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check is Default
			DECLARE @IsDefault BIT;
			DECLARE @BranchId INT = 0;

			SELECT @IsDefault = [IsDefault], @BranchId = [BranchId]
			FROM [dbo].[Stock]
			WHERE [Id] = @Id

			-- If default true 
			-- Then set new default
			IF(@IsDefault = 1)
			BEGIN
				DECLARE @newDefault INT = 0;

				SELECT TOP 1 @newDefault = [Id]
				FROM [dbo].[Stock]
				WHERE [IsDefault] = 0 AND [BranchId] = @BranchId
				ORDER BY [Id] ASC

				UPDATE [dbo].[Stock]
				SET [IsDefault] = 1
				WHERE [Id] = @newDefault
			END

			DELETE [dbo].[Stock]
			WHERE [Id] = @Id

		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_MembershipClass_Delete]
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

			SELECT TOP 1 @IsDefault = [IsDefault]
			FROM [dbo].[MembershipClass]
			WHERE [Id] = @Id

			-- If default true 
			-- Then set new default
			IF(@IsDefault = 1)
			BEGIN
				DECLARE @newDefault INT = 0;

				SELECT TOP 1 @newDefault = [Id]
				FROM [dbo].[MembershipClass]
				WHERE [IsDefault] = 0
				ORDER BY [Level] ASC 

				UPDATE [dbo].[MembershipClass]
				SET [IsDefault] = 1
				WHERE [Id] = @newDefault
			END

			DELETE [dbo].[MembershipClass]
			WHERE [Id] = @Id

			SELECT 1;
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

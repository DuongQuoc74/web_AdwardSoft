CREATE PROCEDURE [dbo].[usp_IPAccess_Update]
	@Id INT,
	@Type TINYINT,
	@Filter TINYINT,
	@IP1 VARCHAR(15),
	@IP2 VARCHAR(15)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[IPAccess]
			SET [Type] = @Type,
				[Filter] = @Filter,
				[IP1] = @IP1,
				[IP2] = @IP2
			WHERE [Id] = @Id
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0
		RETURN 0;
		THROW;
	END CATCH
	SELECT 1
	RETURN 1
END

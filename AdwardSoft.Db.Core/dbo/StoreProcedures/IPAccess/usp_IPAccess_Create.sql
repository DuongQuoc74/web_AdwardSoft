CREATE PROCEDURE [dbo].[usp_IPAccess_Create]
	@Id INT,
	@Type TINYINT,
	@Filter TINYINT,
	@IP1 VARCHAR(15),
	@IP2 VARCHAR(15)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	DECLARE @Return INT = 0
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[IPAccess] ([Type], [Filter], [IP1], [IP2])
			VALUES (@Type, @Filter, @IP1, @IP2)
			SET @Return = IDENT_CURRENT('IPAccess')
		COMMIT		
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SET @Return = 0
		SELECT @Return
		RETURN @Return;
		THROW;
	END CATCH
	SELECT @Return
	RETURN @Return
END

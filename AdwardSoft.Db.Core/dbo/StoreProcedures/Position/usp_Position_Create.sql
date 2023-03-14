CREATE PROCEDURE [dbo].[usp_Position_Create]
	@Id INT,
	@DepartmentId INT,
	@Sort INT,
	@LanguageCode CHAR(2),
	@Title NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO Position (DepartmentId, Sort)
			VALUES (@DepartmentId, IDENT_CURRENT('Position'))

			INSERT INTO PositionTrans ([PositionId], [LanguageCode], [Title])
			VALUES(SCOPE_IDENTITY(), @LanguageCode, @Title)
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		RETURN 0;
		THROW;
	END CATCH
END
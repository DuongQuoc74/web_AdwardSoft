CREATE PROCEDURE [dbo].[usp_Position_Update]
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
			UPDATE Position 
			SET DepartmentId = @DepartmentId
			WHERE Id = @Id
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		RETURN 0;
		THROW;
	END CATCH
END
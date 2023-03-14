CREATE PROCEDURE [dbo].[usp_Department_ReadById]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT d.* 
		FROM [Department] d
		WHERE d.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

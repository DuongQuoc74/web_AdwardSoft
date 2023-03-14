CREATE PROCEDURE [dbo].[usp_Branch_ReadById]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT b.* 
		FROM [Branch] b
		WHERE b.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


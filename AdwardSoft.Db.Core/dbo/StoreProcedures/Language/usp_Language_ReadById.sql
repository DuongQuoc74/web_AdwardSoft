CREATE PROCEDURE [dbo].[usp_Language_ReadById]
	@Id CHAR(2)
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[Language]
		WHERE [Code] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


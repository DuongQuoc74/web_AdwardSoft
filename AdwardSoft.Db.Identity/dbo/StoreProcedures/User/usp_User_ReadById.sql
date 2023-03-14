CREATE PROCEDURE [dbo].[usp_User_ReadById]	
	@Id BIGINT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * FROM [dbo].[User]
		WHERE [Id] = @Id	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

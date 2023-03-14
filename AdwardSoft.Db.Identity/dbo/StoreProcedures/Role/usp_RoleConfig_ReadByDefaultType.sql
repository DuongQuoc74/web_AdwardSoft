CREATE PROCEDURE [dbo].[usp_RoleConfig_ReadByDefaultType]
	@UserType	INT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM		[dbo].[RoleConfig] rc
		INNER JOIN	[Role] r ON rc.[RoleId] = r.[Id]
		WHERE		r.[UserType] = @UserType AND r.[IsDefault] = 1
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_Role_ReadByUserId]
	@Id BIGINT
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT r.*
		FROM [dbo].[UserRole] ur 
		INNER JOIN [dbo].[Role] r ON ur.[RoleId] = r.[Id]
		WHERE ur.[UserId] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_UserRole_Create]
	@Id INT,
	@Name NVARCHAR(30),
	@NormalizedName NVARCHAR(256),
	@ConcurrencyStamp NVARCHAR(MAX),
	@UserId BIGINT

AS
BEGIN 
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[UserRole] ([RoleId], [UserId])
			VALUES (@Id, @UserId);
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		THROW
	END CATCH	
END
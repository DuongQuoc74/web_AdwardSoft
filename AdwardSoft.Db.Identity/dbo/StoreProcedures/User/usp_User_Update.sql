CREATE PROCEDURE [dbo].[usp_User_Update]
	@Id BIGINT,
	@UserName VARCHAR(256),
	@NormalizedUserName VARCHAR(256),
	@Email VARCHAR(256),
	@NormalizedEmail VARCHAR(256),
	@EmailConfirmed BIT,
	@PasswordHash NVARCHAR(max),
	@SecurityStamp NVARCHAR(max),
	@ConcurrencyStamp NVARCHAR(max),
	@PhoneNumber VARCHAR(50),
	@PhoneNumberConfirmed BIT,
	@TwoFactorEnabled BIT,
	@LockoutEndDateUtc DATETIME,
	@LockoutEnabled BIT ,
	@AccessFailedCount INT,
	@FullName NVARCHAR(128),
	@Avatar VARCHAR(256),
	@Type INT,
	@Status INT,
	@Gender INT,
	@IdentityNumber VARCHAR(12)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[User]
			   SET [FullName] =@FullName
				  ,[Avatar] = @Avatar
				  ,[Status] = @Status
				  ,[Gender] = @Gender
			 WHERE Id = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH

	RETURN @return;
END

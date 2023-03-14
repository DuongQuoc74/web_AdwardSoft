CREATE PROCEDURE [dbo].[usp_Role_Create]
	@Id INT,
	@Name NVARCHAR(128),
	@NormalizedName NVARCHAR(256),
	@ConcurrencyStamp NVARCHAR(MAX),
	@IsDefault BIT,
	@UserType INT,
	@IsOTPVerification BIT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	DECLARE @return BIT = 1
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DECLARE @count INT = (SELECT COUNT(*) FROM [Role] WHERE LOWER([Name]) = LOWER(@Name) AND UserType = @UserType)

			IF(@count = 0)
			BEGIN
				INSERT INTO [dbo].[Role]
							(
								[Name],
								[NormalizedName],
								[ConcurrencyStamp],
								[IsDefault],
								[UserType],
								[IsOTPVerification]
							)
				VALUES		(
								@Name,
								@NormalizedName,
								@ConcurrencyStamp,
								@IsDefault,
								@UserType,
								@IsOTPVerification
							)
				SELECT SCOPE_IDENTITY();
			END
			ELSE
			BEGIN
				SET @return = 0
			END
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK;
		THROW;
	END CATCH
	RETURN @return;
	SELECT @return;
END

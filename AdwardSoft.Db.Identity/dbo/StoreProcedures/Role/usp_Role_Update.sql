CREATE PROCEDURE [dbo].[usp_Role_Update]
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
			UPDATE	[dbo].[Role]
			SET		[Name] = @Name,
					[NormalizedName] = @NormalizedName,
					[ConcurrencyStamp] = @ConcurrencyStamp,
					[IsDefault] = @IsDefault,
					[UserType] = @UserType,
					[IsOTPVerification] = @IsOTPVerification
			WHERE	[Id] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
	SELECT @return;
END

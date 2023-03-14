CREATE PROCEDURE [dbo].[usp_User_Create]
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
    @IdentityNumber varchar(12)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
        DECLARE @UserIdx INT = 1;
        SELECT @UserIdx = ISNULL(MAX([UserIdx]), 0) + 1 FROM [dbo].[User] WHERE [Type] = @Type


		INSERT INTO [dbo].[User]
           ([UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[FullName]
           ,[Avatar]
           ,[Type]
           ,[Status]
           ,[Gender]
           ,[UserIdx]
           ,[IdentityNumber])
		 VALUES
           (@UserName
           ,@NormalizedEmail
           ,@Email
           ,@NormalizedEmail
           ,@EmailConfirmed
           ,@PasswordHash
           ,@SecurityStamp
           ,@ConcurrencyStamp
           ,@PhoneNumber
           ,@PhoneNumberConfirmed
           ,@TwoFactorEnabled
           ,@LockoutEndDateUtc
           ,@LockoutEnabled
           ,@AccessFailedCount
           ,@FullName
           ,@Avatar
           ,@Type
           ,@Status
           ,@Gender
           ,@UserIdx
           ,@IdentityNumber)        

           INSERT INTO [UserRole] ([UserId], [RoleId])
           vALUES (SCOPE_IDENTITY(), 2)
		COMMIT	
		RETURN 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		THROW;
	END CATCH
END

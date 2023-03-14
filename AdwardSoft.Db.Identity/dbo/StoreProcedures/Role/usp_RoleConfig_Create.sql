CREATE PROCEDURE [dbo].[usp_RoleConfig_Create]
	@RoleId INT,
	@PwdRegex VARCHAR(150),
	@VerificationMethod tinyint,
	@IsLogging BIT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [RoleConfig] 
			SET PwdRegex = @PwdRegex, VerificationMethod = @VerificationMethod, IsLogging = @IsLogging
			WHERE RoleId = @RoleId

			IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO [RoleConfig] (RoleId, PwdRegex, VerificationMethod, IsLogging)
				VALUES (@RoleId, @PwdRegex, @VerificationMethod, @IsLogging)
			END
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		SELECT 0
		ROLLBACK;
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_MailServer_Create]
	@Id INT,
	@Email VARCHAR(2048), 
    @Name NVARCHAR(150), 
    @SMTP VARCHAR(120), 
    @IsSSL BIT, 
    @IsDefaultCredential BIT,
	@Port INT,
	@Pwd VARCHAR(32)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO MailServer (Email, [Name], [SMTP],[IsSSL], [IsDefaultCredential], [Port], [Pwd])
			VALUES(@Email, @Name, @SMTP, @IsSSL, @IsDefaultCredential, @Port, @Pwd)
		COMMIT
		SELECT SCOPE_IDENTITY()
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
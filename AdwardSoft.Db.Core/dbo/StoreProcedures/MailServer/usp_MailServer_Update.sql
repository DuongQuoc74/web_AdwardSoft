CREATE PROCEDURE [dbo].[usp_MailServer_Update]
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
			UPDATE MailServer 
			SET Email = @Email
			,[Name] = @Name
			,[SMTP] = @SMTP
			,[IsSSL] = @IsSSL
			,[IsDefaultCredential] = @IsDefaultCredential
			,[Port] = @Port
			,[Pwd] = @Pwd
			WHERE Id = @Id
		COMMIT
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_Contact_Create]
	@Id INT,
	@Date SMALLDATETIME,
	@Name NVARCHAR(70),
	@Email VARCHAR(256),
	@Phone VARCHAR(12),
	@Message VARCHAR(256),
	@Status tinyint
AS 
	BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	DECLARE @Return INT = 1
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;		
			INSERT INTO [dbo].[Contact]([Date], [Name], [Email], [Phone], [Message], [Status])
			VALUES (GETDATE(), @Name, @Email, @Phone, @Message, 0)
		COMMIT			
	END TRY
	BEGIN CATCH
		SET @Return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @Return;
END

CREATE PROCEDURE [dbo].[usp_Contact_Update]
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
			UPDATE [dbo].[Contact]
			SET [Status] = @Status
			WHERE [Id] = @Id
		COMMIT			
	END TRY
	BEGIN CATCH
		SET @Return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @Return;
END

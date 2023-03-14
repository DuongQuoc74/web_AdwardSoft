CREATE PROCEDURE [dbo].[usp_CustomerWallet_Delete]
	@Id CHAR(13)
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[CustomerWallet]
			WHERE [Id] = @Id AND [Status] = 0
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0
		THROW;
	END CATCH	
END

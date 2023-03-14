CREATE PROCEDURE [dbo].[usp_Permission_Update]
	@Id INT,
	@Description NVARCHAR(128), 
    @Controller VARCHAR(50), 
    @Action VARCHAR(20)
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[Permission] 
			SET [Description] = @Description,
				[Controller] = @Controller,
				[Action] = @Action
			WHERE [Id] = @Id
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

CREATE PROCEDURE [dbo].[usp_ModuleType_Create]
	@ModuleId INT,
	@Type TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------	
	BEGIN TRY
		BEGIN TRAN;
			INSERT INTO [ModuleType] ([ModuleId], [Type])
			VALUES(@ModuleId, @Type)
		COMMIT	
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_Module_Update]
	@Id INT,
	@Title NVARCHAR(50), 
    @Link VARCHAR(256), 
    @ClassName VARCHAR(50),
	@ControllerName VARCHAR(60),
	@ParentId INT,
	@Sort TINYINT,
	@IsPublic BIT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			UPDATE [dbo].[Module]
			SET [Title] = @Title, [Link] = @Link,[ClassName] = @ClassName, [ControllerName] = @ControllerName, [IsPublic] = @IsPublic
			WHERE [Id] = @Id
			DELETE [ModuleType] WHERE [ModuleId] = @Id
		COMMIT	
		SELECT * FROM Module WHERE Id = @Id
	END TRY
	BEGIN CATCH
		SELECT 0;
		ROLLBACK;
		THROW;
	END CATCH
END
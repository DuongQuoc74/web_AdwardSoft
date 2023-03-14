CREATE PROCEDURE [dbo].[usp_Module_Create]
	@Id INT,
	@Title NVARCHAR(50), 
    @Link VARCHAR(256), 
    @ClassName VARCHAR(50),
	@ControllerName VARCHAR(60),
	@ParentId INT,
	@Sort TINYINT,
	@IsPublic BIT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------	
	BEGIN TRY
		BEGIN TRAN;
			IF(@ParentId <> 0)
			BEGIN
			    SELECT @Sort = COUNT([Sort]) FROM [dbo].[Module] WHERE [ParentId] = @ParentId
				INSERT INTO [dbo].[Module] ([Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic])
				VALUES(@Title, @Link, @ClassName, @ControllerName, @ParentId, @Sort + 1, @IsPublic)
			END
			ELSE
			BEGIN			
				INSERT INTO [dbo].[Module] ([Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic])
				VALUES(@Title, @Link, @ClassName, @ControllerName, IDENT_CURRENT('Module'), 1, @IsPublic)
			END
		COMMIT	
		SELECT * FROM Module WHERE Id = SCOPE_IDENTITY()
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

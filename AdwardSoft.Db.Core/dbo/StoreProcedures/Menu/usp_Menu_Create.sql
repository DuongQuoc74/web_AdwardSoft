CREATE PROCEDURE [dbo].[usp_Menu_Create]
	@Id INT,
	@ParentId INT,
	@Order INT,
	@Type TINYINT,
	@MenuGroupId INT,
	@LanguageCode CHAR(2),
	@NavigationLabel NVARCHAR(70),
	@URL NVARCHAR(2048),
	@ReferenceId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF(@ParentId <> 0)
			BEGIN
				SELECT @Order = COUNT([Order]) FROM [dbo].[Menu] WHERE [ParentId] = @ParentId

				INSERT INTO [dbo].[Menu] ([ParentId], [Order], [MenuGroupId], [Type], [ReferenceId])
				VALUES(@ParentId, @Order + 1, @MenuGroupId, @Type, @ReferenceId)
			END
			ELSE
			BEGIN	
				INSERT INTO [dbo].[Menu] ([ParentId], [Order], [MenuGroupId], [Type], [ReferenceId])
				VALUES(IDENT_CURRENT('Menu'), 1, @MenuGroupId, @Type, @ReferenceId)
			END

			INSERT INTO MenuTranslation([MenuId], [LanguageCode], [NavigationLabel], [URL])
			VALUES(IDENT_CURRENT('Menu'), @LanguageCode, @NavigationLabel, @URL)
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

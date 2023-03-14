CREATE PROCEDURE [dbo].[usp_Menu_Update]
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
			UPDATE [dbo].[Menu]
			SET [Type] = @Type
			WHERE [Id] = @Id
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

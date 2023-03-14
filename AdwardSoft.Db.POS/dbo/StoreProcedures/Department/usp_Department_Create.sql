CREATE PROCEDURE [dbo].[usp_Department_Create]
	@Id INT,
	@Name NVARCHAR(120),
	@Description NVARCHAR(128),
	@ParentId INT,
	@Sort INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF(@ParentId <> 0)
			BEGIN
				SELECT @Sort = COUNT([Sort]) FROM [dbo].[Department] WHERE [ParentId] = @ParentId
				INSERT INTO [dbo].[Department] ([Name],[Description],[ParentId],[Sort])
				VALUES(@Name, @Description, @ParentId, @Sort)
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Department] ([Name],[Description],[ParentId],[Sort])
				VALUES(@Name,@Description,IDENT_CURRENT('Department'),1)
			END
		COMMIT
		RETURN 1
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END

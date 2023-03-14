CREATE PROCEDURE [dbo].[usp_Branch_Create]
	@Id INT,
	@Name NVARCHAR(120),
	@Address NVARCHAR(128),
	@Tel VARCHAR(12),
	@Email VARCHAR(254),
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
				SELECT @Sort = COUNT([Sort]) FROM [dbo].[Branch] WHERE [ParentId] = @ParentId
				INSERT INTO [dbo].[Branch] ([Name],[Address],[Tel],[Email],[ParentId],[Sort])
				VALUES(@Name, @Address, @Tel, @Email, @ParentId, @Sort)
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[Branch] ([Name],[Address],[Tel],[Email],[ParentId],[Sort])
				VALUES(@Name,@Address,@Tel,@Email,IDENT_CURRENT('Branch'),1)
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

CREATE PROCEDURE [dbo].[usp_MenuGroup_Create]
	@Id INT,
    @Name NVARCHAR(150), 
	@Position TINYINT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			INSERT INTO [dbo].[MenuGroup] 
				   ([Name], [Position])
			VALUES (@Name,  IDENT_CURRENT('MenuGroup'))
		COMMIT	
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN 0
		ROLLBACK TRAN
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_Unit_Update]
	@Id INT, 
    @Code CHAR(10), 
    @Name NVARCHAR(10), 
	-- 0.Unavailable (Ngừng hoạt động)
	-- 1.Available (Đang hoạt động)
    @Status BIT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Unit]
			SET [Code] = @Code, 
				[Name] = @Name, 
				[Status] = @Status
			WHERE [Id] = @Id
		COMMIT
		SELECT U.* 
		FROM [dbo].[Unit] AS U
		WHERE U.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
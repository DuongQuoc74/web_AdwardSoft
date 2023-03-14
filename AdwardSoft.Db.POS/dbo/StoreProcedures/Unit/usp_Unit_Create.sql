CREATE PROCEDURE [dbo].[usp_Unit_Create]
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

			INSERT INTO [dbo].[Unit]
					([Name], [Code], [Status])
			VALUES  (@Name , @Code, @Status)

			SET @Id = SCOPE_IDENTITY()

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
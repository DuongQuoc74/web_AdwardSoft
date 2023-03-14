CREATE PROCEDURE [dbo].[usp_Location_Update]
	@Id INT,
    @Code VARCHAR(6),
    @Name NVARCHAR(120), 
    @PostalCode VARCHAR(10), 
    @Rate NUMERIC(3), 
    @ParentId INT, 
    @Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
    BEGIN TRY
        BEGIN TRAN
            UPDATE [dbo].[Location]
            SET [Code] = @Code, 
                [Name] = @Name, 
                [PostalCode] = @PostalCode, 
                [Rate] = @Rate, 
                [ParentId] = @ParentId, 
                [Status] = @Status
            WHERE [Id] = @Id
        COMMIT
        SELECT L.*
        FROM [dbo].[Location] AS L
        WHERE L.[Id] = @Id
    END TRY
    BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_Location_Create]
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
            IF @ParentId <> 0 
                INSERT INTO [dbo].[Location] ([Code], [Name], [PostalCode], [Rate], [ParentId], [Status]) VALUES (@Code, @Name, @PostalCode, @Rate, @ParentId, @Status)
            ELSE
                 INSERT INTO [dbo].[Location] ([Code], [Name], [PostalCode], [Rate], [Status]) VALUES (@Code, @Name, @PostalCode, @Rate, @Status)
            
            SET @Id = SCOPE_IDENTITY()
            
            IF @ParentId = 0 
                UPDATE [dbo].[Location] SET ParentId = @Id WHERE Id = @Id

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

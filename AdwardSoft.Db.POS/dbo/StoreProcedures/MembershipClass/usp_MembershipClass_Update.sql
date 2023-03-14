CREATE PROCEDURE [dbo].[usp_MembershipClass_Update]
	@Id INT, 
    @Name NVARCHAR(60), 
    @LowestValue NUMERIC(16, 2), 
    @HighestValue NUMERIC(16, 2), 
    @Level TINYINT, 
    @IsDefault BIT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[MembershipClass]
			SET	[Name] = @Name, 
				[HighestValue] = @HighestValue,
				[LowestValue] = @LowestValue
			WHERE [Id] = @Id

		COMMIT
		SELECT M.*  
		FROM [dbo].[MembershipClass] AS M
		WHERE M.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
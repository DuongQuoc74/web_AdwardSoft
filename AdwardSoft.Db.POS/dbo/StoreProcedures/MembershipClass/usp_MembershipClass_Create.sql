CREATE PROCEDURE [dbo].[usp_MembershipClass_Create]
	@Id INT, 
    @Name NVARCHAR(60), 
    @LowestValue NUMERIC(16, 2), 
    @HighestValue NUMERIC(16, 2), 
    @Level INT, 
    @IsDefault BIT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check Membership is have default
			DECLARE @Defaulting BIT = 0

			SELECT TOP 1 @Defaulting = 1 
			FROM [dbo].[MembershipClass] 
			WHERE [IsDefault] = 1

			IF(@Defaulting = 0)
				SET @IsDefault = 1

			SELECT @Level = ISNULL(MAX([Level]), 0) FROM [dbo].[MembershipClass]

			IF(@Level = 0)
				SET @Level = 1 
			ELSE
				SET @Level = @Level + 1


			INSERT INTO [dbo].[MembershipClass]
					([Name], [LowestValue], [HighestValue], [Level], [IsDefault])
			VALUES  (@Name , @LowestValue , @HighestValue , @Level , @IsDefault)

			SET @Id = SCOPE_IDENTITY()

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
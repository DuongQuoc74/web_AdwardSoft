CREATE PROCEDURE [dbo].[usp_Setting_Update]
	@Id INT,
    @ProjectName NVARCHAR(120),
    @CompanyName NVARCHAR(120),
    @Address NVARCHAR(120),
    @Tel VARCHAR(10),
    @Website VARCHAR(120),
    @MapCoordinateLat NUMERIC(10,7),
    @MapCoordinateLong NUMERIC(10,7),
    @ErrorRange NUMERIC(3,0)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Setting]
			SET [ProjectName] = @ProjectName,
			    [CompanyName] = @CompanyName,
			    [Tel] = @Tel,
			    [Address] = @Address,
			    [Website] = @Website,
			    [MapCoordinateLat] = @MapCoordinateLat,
			    [MapCoordinateLong] = @MapCoordinateLong,
				[ErrorRange] = @ErrorRange
			WHERE [Id] = @Id
		COMMIT
		SELECT P.* 
		FROM [dbo].[Setting] AS P
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

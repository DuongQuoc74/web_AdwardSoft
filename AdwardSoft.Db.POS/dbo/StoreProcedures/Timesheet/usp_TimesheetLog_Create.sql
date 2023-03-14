CREATE PROCEDURE [dbo].[usp_TimesheetLog_Create]
	@Id INT,
    @UserId BIGINT,
    @Date SMALLDATETIME,
    @Type TINYINT,
    @MapCoordinateLong NUMERIC(10,7),
    @MapCoordinateLat NUMERIC(10,7)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[TimesheetLog]
					([Id], [UserId], [Date], [Type], [MapCoordinateLong], [MapCoordinateLat])
			VALUES  (REPLACE(NEWID(), '-',''), @UserId, GETDATE(), @Type, @MapCoordinateLong, @MapCoordinateLat)
		COMMIT
		SELECT 1
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

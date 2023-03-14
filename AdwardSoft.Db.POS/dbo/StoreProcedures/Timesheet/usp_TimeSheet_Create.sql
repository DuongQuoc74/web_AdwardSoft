CREATE PROCEDURE [dbo].[usp_TimeSheet_Create]
	@UserId BIGINT, 
    @Date DATE, 
    @Type TINYINT,
    @Reason NVARCHAR(150),
    @StartTime VARCHAR(5),
    @EndTime VARCHAR(5),
    @CreatedDate SMALLDATETIME
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[TimeSheet]
				   ([UserId],[Date],[Type],[Reason],[StartTime],[EndTime],[CreatedDate])
			VALUES (@UserId, @Date, @Type, @Reason, @StartTime, @EndTime, @CreatedDate)

		COMMIT
		SELECT 1;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

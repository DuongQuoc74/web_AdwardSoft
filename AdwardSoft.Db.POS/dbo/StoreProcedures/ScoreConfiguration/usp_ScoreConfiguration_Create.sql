CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_Create]
	@Id INT, 
    @EffectiveDate DATE, 
    @FromAmount numeric(16, 2),
    @ToAmount numeric(16, 2),
	@FromPoint numeric(9, 0),
	@ToPoint numeric(9, 0)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[ScoreConfiguration] ([EffectiveDate],[FromAmount],[ToAmount],[FromPoint],[ToPoint])
			VALUES(@EffectiveDate, @FromAmount, @ToAmount,@FromPoint,@ToPoint)

				SET @Id = SCOPE_IDENTITY()
		COMMIT
		SELECT S.* 
		FROM [dbo].[ScoreConfiguration] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

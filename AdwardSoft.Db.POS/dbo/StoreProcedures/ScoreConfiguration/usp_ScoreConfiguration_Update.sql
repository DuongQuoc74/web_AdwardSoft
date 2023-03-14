CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_Update]
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
			UPDATE [dbo].[ScoreConfiguration]
			SET [EffectiveDate] = @EffectiveDate,
				[FromAmount] = @FromAmount,
				[ToAmount] = @ToAmount,
				[FromPoint] = @FromPoint,
				[ToPoint] = @ToPoint
			WHERE [Id] = @Id
		COMMIT
		SELECT S.* 
		FROM [dbo].[ScoreConfiguration] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		RETURN 0;
		THROW;
	END CATCH
END


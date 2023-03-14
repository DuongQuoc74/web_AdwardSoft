CREATE PROCEDURE [dbo].[usp_Promotion_ReadCheckDate]
	@Id INT,
	@Type TINYINT,
    @EffectiveDate DATE, 
    @ExpiryDate DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			IF EXISTS (SELECT TOP 1 1 FROM [dbo].[Promotion] WHERE [Id] != @Id AND [Type] = @Type
				AND (    ([EffectiveDate] <= @EffectiveDate AND  [ExpiryDate] >= @EffectiveDate)
					  OR ([EffectiveDate] <= @ExpiryDate AND  [ExpiryDate] >= @ExpiryDate)
					  OR ([EffectiveDate] >= @EffectiveDate AND  [ExpiryDate] <= @ExpiryDate)
					)
			)
			BEGIN
				SELECT 0
			END
			ELSE
			BEGIN
				SELECT 1
			END
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

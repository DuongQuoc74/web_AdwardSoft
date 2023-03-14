CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_ReadDateIsExisting]
	@Id INT,
	@EffectiveDate DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0;
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[ScoreConfiguration] AS S
		WHERE (S.[EffectiveDate] = @EffectiveDate AND S.[Id] <> @Id) 
		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_ReadById]
@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[ScoreConfiguration]
		WHERE [Id]=@Id
		ORDER BY [EffectiveDate] DESC
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

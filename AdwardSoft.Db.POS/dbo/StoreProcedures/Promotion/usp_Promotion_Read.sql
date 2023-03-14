CREATE PROCEDURE [dbo].[usp_Promotion_Read]
	@Type TINYINT,
	@Status TINYINT,
	@Year INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[Promotion] p
		WHERE (@Type = 4 OR p.[Type] = @Type)
		AND ((@Status = 0 AND p.[ExpiryDate] >= CONVERT(DATE, GETDATE())) OR (@Status = 1 AND p.[ExpiryDate] < CONVERT(DATE, GETDATE())))
		AND @Year = YEAR(p.[EffectiveDate])
		ORDER BY p.[EffectiveDate]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

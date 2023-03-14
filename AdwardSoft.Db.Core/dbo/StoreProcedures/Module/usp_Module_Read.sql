CREATE PROCEDURE [dbo].[usp_Module_Read]	
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT DISTINCT m.*, 
						(
							SELECT ',' + CAST(mt1.[Type] AS char(1)) AS [text()]
							FROM [dbo].[ModuleType] mt1
							WHERE mt1.ModuleId = mt2.ModuleId
							FOR XML PATH('')
						) AS 'Types'
			FROM [dbo].[ModuleType] mt2
			RIGHT JOIN [dbo].[Module] m ON m.Id = mt2.ModuleId
			ORDER BY m.[Sort] DESC
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
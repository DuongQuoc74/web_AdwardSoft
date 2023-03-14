CREATE PROCEDURE [dbo].[usp_Select_ReadEmployeeUser]
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
			SELECT [Id], [UserName] AS [Text] 
			FROM [dbo].[User]					
			WHERE [UserName] NOT IN ('AdwardSoft','Administrator')
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_User_Read]	
	@Type INT,
	@Status INT
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF (@Type = 0)
		BEGIN
			SELECT * FROM [dbo].[User]					
			WHERE [Status] = @Status AND [UserName] NOT IN ('AdwardSoft','Administrator')
		END
		ELSE
		BEGIN
			SELECT * FROM [dbo].[User]					
			WHERE [Status] = @Status AND [UserName] NOT IN ('AdwardSoft','Administrator')
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

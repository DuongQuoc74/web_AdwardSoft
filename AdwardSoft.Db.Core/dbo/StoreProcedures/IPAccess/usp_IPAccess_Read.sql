CREATE PROCEDURE [dbo].[usp_IPAccess_Read]
	@Filter TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF (@Filter = 2)
		BEGIN
			SELECT * FROM [dbo].[IPAccess]
		END
		ELSE
		BEGIN
			SELECT * FROM [dbo].[IPAccess] WHERE [Filter] = @Filter
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

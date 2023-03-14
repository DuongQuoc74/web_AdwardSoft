CREATE PROCEDURE [dbo].[usp_LocationDatatable_Read]
	@ParentId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@Param NVARCHAR(2000) = ' @ParentId INT '
		
		SET @SqlStr = N'SELECT L.*, P.[Name] AS [ParentName]
						FROM [dbo].[Location] AS L
						INNER JOIN [dbo].[Location] AS P ON L.[ParentId] = P.[Id]
						WHERE 1 = 1 '

		BEGIN
			SET @SqlStr= @SqlStr + N' ORDER BY L.[Name] ASC'
			EXEC SP_EXECUTESQL @SqlStr, 
						   @Param,
						   @ParentId
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

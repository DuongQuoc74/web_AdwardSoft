CREATE PROCEDURE [dbo].[usp_Menu_Read]
	@MenuGroupId INT,
	@LanguageCode CHAR(2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	------------------------------------------------
	BEGIN TRY
		IF(@LanguageCode IS NULL)
			SELECT @LanguageCode = [Code] FROM [Language] WHERE [IsDefault] = 1

		IF(@MenuGroupId <> 0)
			SELECT M.*, MT.[NavigationLabel], MT.[LanguageCode], MT.[URL], MG.[Name] AS [MenuGroupName]
			FROM [dbo].[Menu] AS M
			INNER JOIN [dbo].[MenuTranslation] AS MT ON MT.[MenuId] = M.[Id]
			INNER JOIN [dbo].[MenuGroup] AS MG ON MG.[Id] = M.[MenuGroupId]
			WHERE MT.[LanguageCode] = @LanguageCode AND M.[MenuGroupId] = @MenuGroupId
			ORDER BY [Order] DESC
		ELSE
			SELECT M.*, MT.[NavigationLabel], MT.[LanguageCode], MT.[URL], MG.[Name] AS [MenuGroupName]
			FROM [dbo].[Menu] AS M
			INNER JOIN [dbo].[MenuTranslation] AS MT ON MT.[MenuId] = M.[Id]
			INNER JOIN [dbo].[MenuGroup] AS MG ON MG.[Id] = M.[MenuGroupId]
			WHERE MT.[LanguageCode] = @LanguageCode 
			ORDER BY [Order] DESC
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
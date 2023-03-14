CREATE PROCEDURE [dbo].[usp_Menu_ReadCategoryByMenu]
@MenuGroupId INT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT	C.[Id], CT.[Name] AS [NavigationLabel], CT.[Slug] AS [URL], C.[Id] AS [ReferenceId], 2 AS [Type], 
			CASE 
				WHEN M.[Id] IS NULL THEN 0
				ELSE 1
			END AS [IsInMenu]
		FROM	[dbo].[Category] AS C
		INNER JOIN [dbo].[CategoryTranslation] AS CT ON C.[Id] = CT.[CategoryId]
		LEFT JOIN [dbo].[Menu] AS M ON M.[ReferenceId] = C.[Id] AND M.[Type] = 2 AND M.[MenuGroupId] = @MenuGroupId
		INNER JOIN [dbo].[Language] AS L ON L.[Code] = CT.[LanguageCode] AND L.IsDefault = 1
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
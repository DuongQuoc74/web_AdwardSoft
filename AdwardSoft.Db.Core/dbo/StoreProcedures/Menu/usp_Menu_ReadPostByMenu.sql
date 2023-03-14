CREATE PROCEDURE [dbo].[usp_Menu_ReadPostByMenu]
@MenuGroupId INT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT	P.[Id], PT.[Title] AS [NavigationLabel], PT.[Permalink] AS [URL], P.[Id] AS [ReferenceId], 2 AS [Type], 
			CASE 
				WHEN M.[Id] IS NULL THEN 0
				ELSE 1
			END AS [IsInMenu]
		FROM	[dbo].[Post] AS P
		INNER JOIN [dbo].[PostTranslation] AS PT ON P.[Id] = PT.[PostId]
		LEFT JOIN [dbo].[Menu] AS M ON M.[ReferenceId] = P.[Id] AND M.[Type] = 2 AND M.[MenuGroupId] = @MenuGroupId
		INNER JOIN [dbo].[Language] AS L ON L.[Code] = PT.[LanguageCode] AND L.IsDefault = 1
	
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
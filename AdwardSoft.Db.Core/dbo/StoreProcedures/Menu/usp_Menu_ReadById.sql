CREATE PROCEDURE [dbo].[usp_Menu_ReadById]
	@Id INT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT M.*, MT.[NavigationLabel], MT.[LanguageCode], MT.[URL], MG.[Name] AS [MenuGroupName]
		FROM [dbo].[Menu] AS M
		INNER JOIN [dbo].[MenuTranslation] AS MT ON MT.[MenuId] = M.[Id]
		INNER JOIN [dbo].[MenuGroup] AS MG ON MG.[Id] = M.[MenuGroupId]
		WHERE M.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

CREATE PROCEDURE [dbo].[usp_Menu_ReadByLanguage]
	@Id INT,
	@LanguageCode CHAR(2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY

		SELECT MT.*, M.*, MG.[Name] AS [MenuGroupName]
		FROM [dbo].[Menu] AS M
		LEFT JOIN [dbo].[MenuTranslation] AS MT ON M.[Id] = MT.[MenuId] AND MT.[LanguageCode] = @LanguageCode
		INNER JOIN [dbo].[MenuGroup] AS MG ON MG.[Id] = M.[MenuGroupId]
		WHERE M.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

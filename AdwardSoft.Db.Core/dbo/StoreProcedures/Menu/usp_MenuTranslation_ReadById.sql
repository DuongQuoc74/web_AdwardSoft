CREATE PROCEDURE [dbo].[usp_MenuTranslation_ReadById]
	@Id INT,
	@LanguageCode CHAR(2)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT MT.*, M.[ParentId], M.[MenuGroupId], M.[Type], M.[Order], MG.[Name] AS [MenuGroupName]
		FROM [dbo].[MenuTranslation] AS MT
		INNER JOIN [dbo].[Menu] AS M ON M.[Id] = MT.[MenuId]
		INNER JOIN [dbo].[MenuGroup] AS MG ON MG.[Id] = M.[MenuGroupId]
		WHERE [LanguageCode] = @LanguageCode AND [MenuId] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

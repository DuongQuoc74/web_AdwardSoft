CREATE PROCEDURE [dbo].[usp_Select_ReadChildLocation]
(
	@ParentId INT
)
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Location]
	WHERE [Status] = 1 AND [Id] != [ParentId] AND [ParentId] = @ParentId

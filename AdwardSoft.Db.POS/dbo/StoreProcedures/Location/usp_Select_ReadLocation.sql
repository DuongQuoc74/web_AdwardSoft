CREATE PROCEDURE [dbo].[usp_Select_ReadLocation]
(@Status TINYINT = 1)
AS
	SELECT [Id], [Name] AS [Text]
	FROM [dbo].[Location]
	WHERE [Status] = @Status AND [Id] = [ParentId]

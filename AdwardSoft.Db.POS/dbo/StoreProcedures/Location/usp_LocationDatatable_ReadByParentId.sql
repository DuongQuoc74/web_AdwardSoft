CREATE PROCEDURE [dbo].[usp_LocationDatatable_ReadByParentId]
	@ParentId INT
AS
BEGIN
	IF @ParentId != 0
		SELECT L.*, P.[Name] AS [ParentName]
			FROM [dbo].[Location] AS L INNER JOIN [dbo].[Location] AS P ON L.[ParentId] = P.[Id]
			WHERE L.[ParentId] = @ParentId AND L.[Id] != @ParentId
		
	ELSE
		SELECT * FROM [dbo].[Location] WHERE [Id] = [ParentId]
END

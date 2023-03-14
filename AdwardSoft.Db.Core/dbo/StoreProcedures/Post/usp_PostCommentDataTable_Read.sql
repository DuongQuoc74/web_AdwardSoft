CREATE PROCEDURE [dbo].[usp_PostCommentDataTable_Read]
	@type TINYINT,
	@searchBy NVARCHAR(MAX),
	@skip INT,
	@pageSize INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF(@type = 0)
		BEGIN
			SELECT p.[Id], pt.[Title],  
			(SELECT COUNT([Id]) FROM [dbo].[PostComment] WHERE [PostId] = p.[Id]) AS [Comment],
			(SELECT COUNT([Id]) FROM [dbo].[PostComment] WHERE [PostId] = p.[Id] AND [Status] = 0) AS [CommentNA],
			COUNT(p.[Id]) OVER() AS [Count]
			FROM [dbo].[Post] p
			INNER JOIN [dbo].[PostTranslation] pt ON pt.[PostId] = p.[Id]
			INNER JOIN [dbo].[Language] l ON l.[IsDefault] = 1 AND l.[Code] = pt.[LanguageCode]
			INNER JOIN (SELECT TOP 1 * FROM [dbo].[PostComment] ORDER BY (CASE WHEN [Status] = 1 THEN 0 ELSE 1 END), [Status], [Date]) pc
			ON pc.[PostId] = p.[Id]
			ORDER BY pc.[Date] DESC
			OFFSET @skip ROWS
			FETCH NEXT @pageSize ROWS ONLY;	
		END
		ELSE
		BEGIN
			SELECT p.[Id], pt.[Title],  
			(SELECT COUNT([Id]) FROM [dbo].[PostComment] WHERE [PostId] = p.[Id]) AS [Comment],
			(SELECT COUNT([Id]) FROM [dbo].[PostComment] WHERE [PostId] = p.[Id] AND [Status] = 0) AS [CommentNA],
			COUNT(p.[Id]) OVER() AS [Count]
			FROM [dbo].[Post] p
			INNER JOIN [dbo].[PostTranslation] pt ON pt.[PostId] = p.[Id]
			INNER JOIN [dbo].[Language] l ON l.[IsDefault] = 1 AND l.[Code] = pt.[LanguageCode]
			INNER JOIN (SELECT TOP 1 * FROM [dbo].[PostComment] WHERE [Status] = 0 ORDER BY [Date]) pc
			ON pc.[PostId] = p.[Id]
			ORDER BY pc.[Date] DESC
			OFFSET @skip ROWS
			FETCH NEXT @pageSize ROWS ONLY;
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_PostComment_Read]
	@parentId CHAR(32),
	@postId BIGINT,
	@status INT,
	@skip INT,
	@pageSize INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF(@parentId = '')
		BEGIN
			SELECT  pc.*, u.[FullName] AS [UserName], u.[Avatar] AS [Avatar],
			COUNT(pc.[Id]) OVER() AS [Count]
			FROM [dbo].[PostComment] pc
			INNER JOIN [ADSIdentity.GS].[dbo].[User] u ON u.[Id] = pc.[UserId]
			WHERE pc.[PostId] = @postId AND ((pc.[Status] = @status) 
			OR (pc.[Id] IN (SELECT p.[Id] 
							FROM [dbo].[PostComment] p
							INNER JOIN [dbo].[PostComment] c ON p.[Id] = c.[ParentId]
							WHERE c.[Status] = @status AND c.[Id] != c.[ParentId])))
 			ORDER BY pc.[Date] DESC
			OFFSET @skip ROWS
			FETCH NEXT @pageSize ROWS ONLY;	
		END
		ELSE
		BEGIN
			SELECT  pc.*, u.[FullName] AS [UserName], u.[Avatar] AS [Avatar],
			COUNT(pc.[Id]) OVER() AS [Count]
			FROM [dbo].[PostComment] pc
			INNER JOIN [ADSIdentity.GS].[dbo].[User] u ON u.[Id] = pc.[UserId]
			WHERE pc.[ParentId] = @parentId AND pc.[Status] = @status
			ORDER BY pc.[Date] DESC
			OFFSET @skip ROWS
			FETCH NEXT @pageSize ROWS ONLY;
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

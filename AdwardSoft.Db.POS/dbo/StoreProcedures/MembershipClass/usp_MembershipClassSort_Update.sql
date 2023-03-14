CREATE PROCEDURE [dbo].[usp_MembershipClassSort_Update]
	@SelectId INT,
	@IsUp BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			SELECT [Id], [Level], LAG([Level]) OVER (ORDER BY [Level]) AS 'previousSort', LEAD([Level]) OVER (ORDER BY [Level]) AS 'nextSort', 
			LAG([Id]) OVER (ORDER BY [Level]) AS 'previousId', LEAD([Id]) OVER (ORDER BY [Level]) AS 'nextId'
			INTO #temp
			FROM [dbo].[MembershipClass]

			IF ( @IsUp = 1 ) AND EXISTS ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectId AND [previousId] IS NOT NULL)
			BEGIN
				UPDATE [MembershipClass]
				SET [Level] = tmp.[previousSort]
				FROM [MembershipClass] AS p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE p.[Id] = @SelectId

				UPDATE [MembershipClass]
				SET [Level] = tmp.[nextSort]
				FROM [MembershipClass] AS p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE tmp.[Id] IN ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectId)
			END
			ELSE IF ( @IsUp = 0 ) AND EXISTS ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectId AND [nextId] IS NOT NULL)
			BEGIN
				UPDATE [MembershipClass]
				SET [Level] = tmp.[nextSort]
				FROM [MembershipClass] AS p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE p.[Id] = @SelectId

				UPDATE [MembershipClass]
				SET [Level] = tmp.[previousSort]
				FROM [MembershipClass] AS p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE tmp.[Id] IN ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectId)
			END
			-- Get top level
			DECLARE @newDefault INT = 0

			SELECT TOP 1 @newDefault = [Id]
			FROM [dbo].[MembershipClass]
			ORDER BY [Level] ASC 

			-- Remove current Default
			UPDATE [dbo].[MembershipClass]
			SET	[IsDefault] = 0

			-- SET new Default
			UPDATE [dbo].[MembershipClass]
			SET	[IsDefault] = 1
			WHERE [Id] = @newDefault



			DROP TABLE IF EXISTS #temp
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

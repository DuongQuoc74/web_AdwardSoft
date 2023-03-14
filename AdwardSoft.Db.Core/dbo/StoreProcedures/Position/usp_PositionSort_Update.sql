CREATE PROCEDURE [dbo].[usp_PositionSort_Update]
	@SelectId INT,
	@IsUp BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			SELECT [Id], [Sort], LAG([Sort]) OVER (ORDER BY [Sort]) AS 'previousSort', LEAD([Sort]) OVER (ORDER BY [Sort]) AS 'nextSort', 
			LAG([Id]) OVER (ORDER BY [Sort]) AS 'previousId', LEAD([Id]) OVER (ORDER BY [Sort]) AS 'nextId'
			INTO #temp
			FROM [Position]

			IF ( @IsUp = 1 ) AND EXISTS ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectId AND [previousId] IS NOT NULL)
			BEGIN
				UPDATE [Position]
				SET [Sort] = tmp.[previousSort]
				FROM [Position] p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE p.[Id] = @SelectId

				UPDATE [Position]
				SET [Sort] = tmp.[nextSort]
				FROM [Position] p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE tmp.[Id] IN ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectId)
			END
			ELSE IF ( @IsUp = 0 ) AND EXISTS ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectId AND [nextId] IS NOT NULL)
			BEGIN
				UPDATE [Position]
				SET [Sort] = tmp.[nextSort]
				FROM [Position] p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE p.[Id] = @SelectId

				UPDATE [Position]
				SET [Sort] = tmp.[previousSort]
				FROM [Position] p
				INNER JOIN #temp tmp ON tmp.[Id] = p.[Id]
				WHERE tmp.[Id] IN ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectId)
			END

			DROP TABLE IF EXISTS #temp
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END
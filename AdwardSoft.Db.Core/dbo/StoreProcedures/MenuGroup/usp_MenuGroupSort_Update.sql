CREATE PROCEDURE [dbo].[usp_MenuGroupSort_Update]
	@SelectedId INT,
	@IsUp BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			SELECT [Id], [Position], LAG([Position]) OVER (ORDER BY [Position]) AS 'previousSort', 
				   LEAD([Position]) OVER (ORDER BY [Position]) AS 'nextSort', 
			       LAG([Id]) OVER (ORDER BY [Position]) AS 'previousId', 
				   LEAD([Id]) OVER (ORDER BY [Position]) AS 'nextId'
			INTO #temp
			FROM [dbo].[MenuGroup]

			IF ( @IsUp = 1 ) AND EXISTS ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectedId AND [previousId] IS NOT NULL)
			BEGIN
				UPDATE [dbo].[MenuGroup]
				SET [Position] = tmp.[previousSort]
				FROM [dbo].[MenuGroup] AS MG
				INNER JOIN #temp AS tmp ON tmp.[Id] = MG.[Id]
				WHERE MG.[Id] = @SelectedId

				UPDATE [dbo].[MenuGroup]
				SET [Position] = tmp.[nextSort]
				FROM [dbo].[MenuGroup] AS MG
				INNER JOIN #temp AS tmp ON tmp.[Id] = MG.[Id]
				WHERE tmp.[Id] IN ( SELECT [previousId] FROM #temp WHERE [Id] = @SelectedId)
			END
			ELSE IF ( @IsUp = 0 ) AND EXISTS ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectedId AND [nextId] IS NOT NULL)
			BEGIN
				UPDATE [dbo].[MenuGroup]
				SET [Position] = tmp.[nextSort]
				FROM [dbo].[MenuGroup] AS MG
				INNER JOIN #temp as tmp ON tmp.[Id] = MG.[Id]
				WHERE MG.[Id] = @SelectedId

				UPDATE [dbo].[MenuGroup]
				SET [Position] = tmp.[previousSort]
				FROM [dbo].[MenuGroup] as MG
				INNER JOIN #temp AS tmp ON tmp.[Id] = MG.[Id]
				WHERE tmp.[Id] IN ( SELECT [nextId] FROM #temp WHERE [Id] = @SelectedId)
			END

			DROP TABLE IF EXISTS #temp
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END
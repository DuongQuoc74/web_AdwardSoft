CREATE PROCEDURE [dbo].[usp_PostDetail_ReadById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT tmp.*, pg.[Image]
		FROM (
			SELECT p.*, LAG(p.[Id]) OVER (ORDER BY p.[PublishedOn] DESC, p.[Id]) AS 'PreviousId',
			LEAD(p.[Id]) OVER (ORDER BY p.[PublishedOn] DESC, p.[Id]) AS 'NextId',
			LAG(p.[Title]) OVER (ORDER BY p.[PublishedOn] DESC, p.[Id]) AS 'PreviousTitle',
			LEAD(p.[Title]) OVER (ORDER BY p.[PublishedOn] DESC, p.[Id]) AS 'NextTitle',
			ps.[MetaRobotAdvanced], ps.[MetaRobotFollow], ps.[MetaRobotIndex]
			FROM Post p
			LEFT JOIN PostSEO ps ON p.[Id] = ps.[PostId]
			WHERE p.[Status] = 2
		) tmp
		LEFT JOIN PostGallery pg ON tmp.[Id] = pg.[PostId]
		WHERE tmp.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

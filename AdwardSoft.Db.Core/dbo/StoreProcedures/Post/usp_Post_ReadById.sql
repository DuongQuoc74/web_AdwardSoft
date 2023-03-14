CREATE PROCEDURE [dbo].[usp_Post_ReadById]
	@Id INT
AS 
	BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT p.*, ps.[Title] Title, ps.[Description] DescriptionSEO, ps.[CanonicalURL], ps.[MetaRobotIndex], ps.[MetaRobotFollow], ps.[MetaRobotAdvanced]
		FROM [Post] p
		INNER JOIN [PostSEO] ps ON p.[Id] = ps.[PostId]
		WHERE p.[Id] = @Id 
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
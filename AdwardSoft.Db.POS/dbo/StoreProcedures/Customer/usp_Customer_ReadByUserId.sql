CREATE PROCEDURE [dbo].[usp_Customer_ReadByUserId]
	@UserId BIGINT
AS
	SELECT TOP 1 *
	FROM [dbo].[Customer]
	WHERE [UserId] = @UserId
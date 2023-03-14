CREATE PROCEDURE [dbo].[usp_Language_ReadByDefault]
AS
	SELECT * 
	FROM [dbo].[Language]
	WHERE [IsDefault] = 1

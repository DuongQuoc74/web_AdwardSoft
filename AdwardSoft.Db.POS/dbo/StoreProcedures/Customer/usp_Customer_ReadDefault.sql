CREATE PROCEDURE [dbo].[usp_Customer_ReadDefault]
AS
	SELECT * 
	FROM [dbo].[Customer]
	WHERE [IsDefault] = 1

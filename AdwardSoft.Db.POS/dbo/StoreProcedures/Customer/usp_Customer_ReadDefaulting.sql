CREATE PROCEDURE [dbo].[usp_Customer_ReadDefaulting]
AS
	SELECT TOP 1 * 
	FROM [dbo].[Customer]
	WHERE [IsDefault] = 1
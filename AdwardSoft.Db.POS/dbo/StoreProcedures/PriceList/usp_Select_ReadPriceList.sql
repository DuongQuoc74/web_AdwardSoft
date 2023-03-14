CREATE PROCEDURE [dbo].[usp_Select_ReadPriceList]
AS
	SELECT [Date], [Title] AS [Text]
	FROM [dbo].[PriceList]

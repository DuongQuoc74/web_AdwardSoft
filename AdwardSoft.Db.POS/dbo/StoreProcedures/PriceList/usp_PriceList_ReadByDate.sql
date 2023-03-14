CREATE PROCEDURE [dbo].[usp_PriceList_ReadByDate]
	@Date DATE
AS
	SELECT *
	FROM [dbo].[PriceList]
	WHERE [Date] = @Date


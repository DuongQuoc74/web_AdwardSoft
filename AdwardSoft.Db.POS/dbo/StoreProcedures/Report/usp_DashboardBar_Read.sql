CREATE PROCEDURE [dbo].[usp_DashboardBar_Read]
@Type TINYINT = 1
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

	--Total sales
	IF(@Type = 1)
	BEGIN
		SELECT MONTH([CreateDate]) AS [Month], SUM([TotalAmount]) AS [Amount]
		FROM [dbo].[SaleReceipt] 
		WHERE YEAR([CreateDate]) = YEAR(GETDATE())
		GROUP BY MONTH([CreateDate])
		ORDER BY MONTH([CreateDate])
	END
END
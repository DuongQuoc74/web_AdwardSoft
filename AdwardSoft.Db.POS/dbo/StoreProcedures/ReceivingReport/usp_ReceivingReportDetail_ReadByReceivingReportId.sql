CREATE PROCEDURE [dbo].[usp_ReceivingReportDetail_ReadByReceivingReportId]
	@ReceivingReportId CHAR(32)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		SELECT r.*, p.[Name] AS [ProductName], p.[Code] AS [ProductCode]
		FROM [dbo].[ReceivingReportDetail] r
		INNER JOIN [dbo].[Product] p ON p.[Id] = r.[ProductId]
		WHERE r.[ReceivingReportId] = @ReceivingReportId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

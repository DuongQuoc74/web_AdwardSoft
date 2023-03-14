CREATE PROCEDURE [dbo].[usp_PriceList_Read]
	@DateFrom DATE,
	@DateTo DATE
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @DateFrom DATETIME,
											  @DateTo DATETIME '

		SET @SqlStr = N' SELECT *
						 FROM [dbo].[PriceList]
						 WHERE @DateFrom <=  CONVERT(DATE, [Date]) AND  @DateTo >=  CONVERT(DATE, [Date]) '

		SET @SqlStr= @SqlStr + N' ORDER BY [Date] DESC '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @DateFrom,
						   @DateTo

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END



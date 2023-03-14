CREATE PROCEDURE [dbo].[usp_SaleReceiptHistory_ReadBak]
	@searchBy NVARCHAR(50),
	@ShiftId BIGINT,
	@CheckoutCounterId BIGINT,
	@DateFrom DATE,
	@DateTo DATE
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr1 NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '
												@searchBy NVARCHAR(50),
												@ShiftId BIGINT,
												@CheckoutCounterId BIGINT,
												@DateFrom DATE,
												@DateTo DATE '


		SET @SqlStr1 = N'SELECT sr.* , c.[Name] AS [CustomerName]
						, COUNT(*) OVER() AS [Count]
						FROM [dbo].[SaleReceipt] sr
						INNER JOIN [dbo].[Customer] c ON c.[Id] = sr.[CustomerId]
						WHERE @DateFrom <=  CONVERT(DATE, sr.[Date]) AND  @DateTo >=  CONVERT(DATE, sr.[Date]) ';

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND (sr.[No] LIKE ''%''+@searchBy+''%'') '
		END

		IF(@ShiftId <> 0)
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND sr.[ShiftId] = @ShiftId '
		END

		IF(@CheckoutCounterId <> 0)
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND sr.[CheckoutCounterId] = @CheckoutCounterId '
		END

		SET @SqlStr1 = @SqlStr1 +  N' ORDER BY sr.[Date] DESC, sr.[No]'
		EXEC SP_EXECUTESQL @SqlStr1, 
						   @ParamList,
						   @searchBy,
						   @ShiftId,
						   @CheckoutCounterId,
						   @DateFrom,
						   @DateTo
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

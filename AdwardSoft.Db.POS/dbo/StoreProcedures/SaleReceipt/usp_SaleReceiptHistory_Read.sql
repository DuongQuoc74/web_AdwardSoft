CREATE PROCEDURE [dbo].[usp_SaleReceiptHistory_Read]
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
						, COUNT(*) OVER() AS [Count],
						srd.*, u.[Name] as UnitName, (CASE WHEN srd.[CardPinCode] IS NULL THEN p.[Name] ELSE RTRIM(p.[Name]) + '' ('' + RTRIM(srd.[CardPinCode]) + '' - '' + RTRIM(srd.[CardSerial]) + '')'' END
						) as ProductName, 
						p.[Code] as [ProductCode]
						FROM [dbo].[SaleReceiptDetail] srd
						LEFT JOIN [dbo].[Unit] u ON srd.[UnitId] = u.[Id]
						LEFT JOIN [dbo].[Product] p ON srd.[ProductId] = p.[Id]
						LEFT JOIN [dbo].[SaleReceipt] sr ON srd.[SaleReceiptId] = sr.[Id]
						INNER JOIN [dbo].[Customer] c ON c.[Id] = sr.[CustomerId]
						WHERE @DateFrom <=  CONVERT(DATE, sr.[Date]) AND  @DateTo >=  CONVERT(DATE, sr.[Date]) ';

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND (sr.[No] LIKE ''%''+@searchBy+''%'' OR c.[Name] LIKE ''%'' + @searchBy+''%'' OR p.[Name] LIKE ''%''+@searchBy+''%'' OR p.[Code] LIKE ''%''+@searchBy+''%'') '
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

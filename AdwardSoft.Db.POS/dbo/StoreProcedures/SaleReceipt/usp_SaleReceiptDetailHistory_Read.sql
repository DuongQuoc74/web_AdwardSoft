CREATE PROCEDURE [dbo].[usp_SaleReceiptDetailHistory_Read]
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


		SET @SqlStr1 = N'SELECT srd.*, u.[Name] as UnitName, p.[Name] as ProductName, p.[Code] as ProductCode
						FROM [dbo].[SaleReceiptDetail] srd
						LEFT JOIN [dbo].[Unit] u ON srd.[UnitId] = u.[Id]
						LEFT JOIN [dbo].[Product] p ON srd.[ProductId] = p.[Id]
						WHERE [SaleReceiptId] IN (
						SELECT Id
						FROM [dbo].[SaleReceipt] sr
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
		SET @SqlStr1 = @SqlStr1 + ')'
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

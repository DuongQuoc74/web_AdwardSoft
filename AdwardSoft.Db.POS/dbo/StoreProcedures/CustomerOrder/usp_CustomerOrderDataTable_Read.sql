CREATE PROCEDURE [dbo].[usp_CustomerOrderDatatable_Read]
	@CustomerId INT,
	@Status TINYINT,
	@PaymentStatus TINYINT,
	@DeliveryPointId INT,
	@DateFrom DATETIME,
	@DateTo DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @CustomerId INT,
											  @Status TINYINT,
											  @PaymentStatus TINYINT,
											  @DeliveryPointId INT,
											  @DateFrom DATETIME,
											  @DateTo DATETIME '

		SET @SqlStr = N' SELECT C.[Date], C.[Id], C.[Receiver], C.[TotalQuantityReg], C.[TotalQuantity], C.[TotalAmount], C.[PaymentStatus], C.[Status], D.[Name] AS [CustomerName]
						 FROM [dbo].[CustomerOrder] AS C INNER JOIN [dbo].[Customer] AS D ON C.[CustomerId] = D.[Id] 
						 WHERE @DateFrom <= CONVERT(DATE, [Date]) AND  @DateTo >= CONVERT(DATE, [Date]) '

		IF @CustomerId <> 0 
			SET @SqlStr= @SqlStr + N' AND [CustomerId] = @CustomerId '

		IF @DeliveryPointId <> 0 
			SET @SqlStr= @SqlStr + N' AND [DeliveryPointId] = @DeliveryPointId '

		IF @Status <> 5
			SET @SqlStr= @SqlStr + N' AND [Status] = @Status '

		IF @PaymentStatus <> 5
			SET @SqlStr= @SqlStr + N' AND [PaymentStatus] = @PaymentStatus '

		SET @SqlStr= @SqlStr + N' ORDER BY [Date] DESC '
				
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @CustomerId,
						   @Status,
						   @PaymentStatus,
						   @DeliveryPointId,
						   @DateFrom,
						   @DateTo

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END



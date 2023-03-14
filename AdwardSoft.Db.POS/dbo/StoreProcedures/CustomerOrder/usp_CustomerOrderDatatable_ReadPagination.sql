CREATE PROCEDURE [dbo].[usp_CustomerOrderDatatable_ReadPagination]
	@PageSize INT,
	@PageSkip INT,
	-- Default Value Is 255 Get All Order Except Paid Payment
	@PaymentStatus TINYINT = 255,
	@FromDate SMALLDATETIME,
	@ToDate SMALLDATETIME
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		DECLARE @SqlCommand NVARCHAR(MAX),
				@ParamList NVARCHAR(1000) = ' @PageSize INT, 
											  @PageSkip INT,
											  @PaymentStatus TINYINT,
											  @FromDate SMALLDATETIME,
											  @ToDate SMALLDATETIME'

		SET @SqlCommand = N'SELECT CO.*, C.[Name] AS [CustomerName]
							FROM [dbo].[CustomerOrder] AS CO
							INNER JOIN [dbo].[Customer] AS C ON CO.[CustomerId] = C.[Id]
							WHERE CO.[Date] >= @FromDate AND CO.[Date] <= @ToDate'
		
		-- If Payment Status != 255 => Load By PaymentStatus
		IF (@PaymentStatus <> 255)
			SET @SqlCommand = @SqlCommand + N' AND CO.[PaymentStatus] = @PaymentStatus '
		ELSE 
		-- If Equals => Load All Except Paid Payment
			SET @SqlCommand = @SqlCommand + N' AND CO.[PaymentStatus] != 1 '

		-- Sory by create date and modified date
		SET @SqlCommand = @SqlCommand + N' ORDER BY CO.[Date], CO.[ModifiedDate]
								  OFFSET @PageSkip ROWS 
								  FETCH NEXT @PageSize ROWS ONLY '

		EXEC SP_EXECUTESQL  @SqlCommand,
							@ParamList,
							@PageSize,
							@PageSkip,
							@PaymentStatus,
							@FromDate,
							@ToDate
	END TRY

	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_CustomerOrderDetail_Create]
	@OrderId CHAR(32),
	@ProductId INT,
	@QuantityReg NUMERIC(8,3),
	@Quantity NUMERIC(8,3),
	@UnitId INT,
	@Price NUMERIC(16,2),
	@Discount NUMERIC(16,2) = 0,
	@Amount NUMERIC(16,2),
	@IsPromo BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN

			IF @OrderId IS NOT NULL
			BEGIN
				SET @Quantity = @QuantityReg
				SET @Amount = @Quantity * @Price - @Discount

				INSERT INTO [dbo].[CustomerOrderDetails] 
				([OrderId], [ProductId], [QuantityReg], [Quantity], [UnitId], [Price], [Discount], [Amount], [IsPromo])
				VALUES
				(@OrderId, @ProductId, @QuantityReg, @Quantity, @UnitId, @Price, @Discount, @Amount, @IsPromo)
			END

		COMMIT

		SELECT C.* 
		FROM [dbo].[CustomerOrderDetails] AS C
		WHERE C.[OrderId] = @OrderId AND C.[ProductId] = @ProductId
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

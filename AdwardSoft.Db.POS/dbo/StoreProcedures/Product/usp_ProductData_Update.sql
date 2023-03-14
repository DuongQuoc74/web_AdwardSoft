CREATE PROCEDURE [dbo].[usp_ProductData_Update]
	@Id INT, 
	@Code CHAR(13),
    @Name NVARCHAR(120), 
    @Image NVARCHAR(128), 
    @MinStock NUMERIC(13,3), 
    @MaxStock NUMERIC(13,3),
	@UnitId INT,	
	@StockId INT,
	@Status TINYINT,
	@CategoryId INT,
	@IsDefault BIT,
	@WholesalePrice NUMERIC(16, 2),
	@RetailPrice NUMERIC(16, 2),
	@PriceType TINYINT,
	@IsTrade BIT,
	@Profit NUMERIC(5,3)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Product]
			SET [Name] = @Name,
				[Code]=@Code,
				[Image] = @Image,
				[MinStock] = @MinStock,
				[MaxStock] = @MaxStock,
				[UnitId]=@UnitId,
				[StockId]=@StockId,
				[Status]=@Status,
				[IsTrade] = @IsTrade,
				[PriceType] = @PriceType,
				[Profit] = @Profit,
				[ModifyDate] = GETDATE()
			WHERE [Id] = @Id

			UPDATE [dbo].[ProductCategory]
			SET [CategoryId]=@CategoryId
			WHERE [ProductId]=@Id

			--IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[SellingPrice] WHERE [ProductId] = @Id AND [UnitId] = @UnitId  AND CONVERT(DATE, [Date]) = CONVERT(DATE, GETDATE()))
			--BEGIN
			--	INSERT INTO [dbo].[SellingPrice] ([ProductId], [Date], [UnitId], [WholesalePrice], [RetailPrice] )
			--	VALUES(@Id, GETDATE(), @UnitId, @WholesalePrice, @RetailPrice)
			--END
			--ELSE
			--BEGIN
			--	UPDATE [dbo].[SellingPrice]
			--	   SET [WholesalePrice] = @WholesalePrice,
			--		   [RetailPrice] = @RetailPrice
			--	WHERE  [ProductId] = @Id AND [UnitId] = @UnitId AND CONVERT(DATE, [Date]) = CONVERT(DATE, GETDATE())
			--END
		COMMIT
		SELECT P.* 
		FROM [dbo].[Product] AS P
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

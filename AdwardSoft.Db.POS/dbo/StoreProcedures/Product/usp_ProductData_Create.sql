CREATE  PROCEDURE [dbo].[usp_ProductData_Create]
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
	@Profit NUMERIC(5,3),
	@IsTrade BIT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
				IF(@Code IS NULL)
					SET @Code = ''
				

				INSERT INTO [dbo].[Product] ([Code],[Name],[Image],[MinStock],[MaxStock],
				[UnitId],[StockId],[Status], [PriceType], [IsTrade],[ModifyDate],[Profit])
				VALUES(@Code,@Name, @Image, @MinStock, @MaxStock,@UnitId,@StockId,@Status,
				@PriceType, @IsTrade, GETDATE(), @Profit)

				SET @Id = SCOPE_IDENTITY()
				IF @Code = '' --Internal Code
				BEGIN
					SET @Code ='8798'+ REPLACE(STR(@Id, 8), SPACE(1), '0')
					--Checksum - Adward
					DECLARE @Checksum INT = 0
					SELECT @Checksum= (10-(
						CAST(SUBSTRING(@Code,1,1) AS int)+
						CAST(SUBSTRING(@Code,2,1) AS int)*3+
						CAST(SUBSTRING(@Code,3,1) AS int)+
						CAST(SUBSTRING(@Code,4,1) AS int)*3+
						CAST(SUBSTRING(@Code,5,1) AS int)+
						CAST(SUBSTRING(@Code,6,1) AS int)*3+
						CAST(SUBSTRING(@Code,7,1) AS int)+
						CAST(SUBSTRING(@Code,8,1) AS int)*3+
						CAST(SUBSTRING(@Code,9,1) AS int)+
						CAST(SUBSTRING(@Code,10,1) AS int)*3+
						CAST(SUBSTRING(@Code,11,1) AS int)+
						CAST(SUBSTRING(@Code,12,1) AS int)*3)%10)%10
					SET @Code = SUBSTRING(@Code,1,12) + CAST(@Checksum AS CHAR(1))
					--Update
					UPDATE [dbo].[Product]	
					SET [Code] = @Code
					WHERE [Id] = @Id
				END


				INSERT INTO [dbo].[ProductCategory] ( [ProductId], [CategoryId], [IsDefault] )
				VALUES ( @Id, @CategoryId, @IsDefault )

				--INSERT INTO [dbo].[SellingPrice] ( [ProductId], [Date] , [UnitId], [WholesalePrice],[RetailPrice])
				--VALUES (@Id, GETDATE(), @UnitId, @WholesalePrice, @RetailPrice)			
		COMMIT
		SELECT P.* 
		FROM [dbo].[Product] AS P
		WHERE P.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END
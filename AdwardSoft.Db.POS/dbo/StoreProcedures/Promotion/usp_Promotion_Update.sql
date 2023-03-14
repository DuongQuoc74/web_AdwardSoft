CREATE PROCEDURE [dbo].[usp_Promotion_Update]
	@Id INT, 
    @Name NVARCHAR(128), 
    @EffectiveDate DATE, 
    @ExpiryDate NVARCHAR(100), 
    @Type TINYINT
	-- 0 : Donation with purchase (Mua hàng tặng hàng)
	-- 1 : Discount on total value (Giảm giá trên tổng giá trị đơn hàng)
	-- 2 : Disount on product (Giảm giá trực tiếp trên sản phẩm)
	-- 3 : Discount for combo (Giảm giá mua hàng kết hợp) 
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[Promotion]
			SET [Name] = @Name,
				[EffectiveDate] = @EffectiveDate,
				[ExpiryDate] = @ExpiryDate,
				[Type] = @Type
			WHERE [Id] = @Id
		COMMIT
	END TRY
	BEGIN CATCH
		SELECT 0;
		RETURN 0;
		ROLLBACK;
		THROW;
	END CATCH
	SELECT 1;
	RETURN 1;
END
CREATE PROCEDURE [dbo].[usp_Promotion_Create]
	@Id INT, 
    @Name NVARCHAR(128), 
    @EffectiveDate DATE, 
    @ExpiryDate DATE, 
    @Type TINYINT
	-- 0 : Donation with purchase (Mua hàng tặng hàng)
	-- 1 : Discount on total value (Giảm giá trên tổng giá trị đơn hàng)
	-- 2 : Disount on product (Giảm giá trực tiếp trên sản phẩm)
	-- 3 : Discount for combo (Giảm giá mua hàng kết hợp) 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[Promotion]
					([Name], [EffectiveDate], [ExpiryDate], [Type])
			VALUES  (@Name , @EffectiveDate, @ExpiryDate, @Type)			
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

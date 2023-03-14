CREATE PROCEDURE [dbo].[usp_CustomerGroupDatatable_Read]
	-- 0.Wholesale Price (Giá bán sỉ)
	-- 1.Retail Price (Giá bán lẻ)
	@PricingPolicy INT = -1,
	-- 0.Unavailable (Ngừng hoạt động)
	-- 1.Available (Đang hoạt động)
	@Status INT = -1
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '@PricingPolicy INT,
											  @Status INT '
		
		SET @SqlStr = N'SELECT C.* 
						FROM [dbo].[CustomerGroup] AS C
						WHERE 1 = 1 '

		IF(@PricingPolicy = 0)
			SET @SqlStr = @SqlStr + N' AND C.[PricingPolicy] = 0 '
		IF(@PricingPolicy = 1)
			SET @SqlStr = @SqlStr + N' AND C.[PricingPolicy] = 1 '

		IF(@Status = 0)
			SET @SqlStr = @SqlStr + N' AND C.[Status] = 0 '
		IF(@Status = 1)
			SET @SqlStr = @SqlStr + N' AND C.[Status] = 1 '


		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @PricingPolicy,
						   @Status
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
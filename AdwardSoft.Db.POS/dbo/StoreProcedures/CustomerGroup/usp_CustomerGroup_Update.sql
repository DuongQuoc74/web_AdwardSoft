CREATE PROCEDURE [dbo].[usp_CustomerGroup_Update]
	@Id INT, 
    @Name NVARCHAR(60), 
    @Note NVARCHAR(100), 
	-- 0. Wholesale Price (Giá bán sỉ)
	-- 1. Retail Price (Giá bán lẻ)
    @PricingPolicy TINYINT,
	-- 0. Unavailable (Ngừng hoạt động)
	-- 1. Available (Đang hoạt động)
    @Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[CustomerGroup]
			SET [Name] = @Name, 
				[Note] = @Note, 
				[PricingPolicy] = @PricingPolicy,
				[Status] = @Status
			WHERE [Id] = @Id
		COMMIT
		SELECT C.* 
		FROM [dbo].[CustomerGroup] AS C
		WHERE C.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
﻿CREATE PROCEDURE [dbo].[usp_CustomerGroup_Create]
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

			INSERT INTO [dbo].[CustomerGroup]
					([Name], [Note], [PricingPolicy], [Status])
			VALUES  (@Name , @Note, @PricingPolicy, @Status)

			SET @Id = SCOPE_IDENTITY()

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
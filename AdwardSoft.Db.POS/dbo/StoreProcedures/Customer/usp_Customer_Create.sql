CREATE PROCEDURE [dbo].[usp_Customer_Create]
	@Id INT, 
    @Name NVARCHAR(120), 
    -- Business License (GPKD)
    -- Identity Card (CMND)
    -- Citizenship Card (CCCD)
    -- Passport (Hộ chiếu)
    @IdentityCode VARCHAR(20), 
    -- Personal – Cá nhân
    -- Organization – Tổ chức 
    @Type TINYINT, 
    @Phone VARCHAR(12), 
    @Email VARCHAR(254), 
    @Address NVARCHAR(128), 
    @PaymentMethodId INT, 
    @CustomerGroupId INT, 
    --@MembershipClassId INT, 
    @IsInvoice BIT, 
    @Note NVARCHAR(100),
    -- 0. Unavailable (Ngừng hoạt động) 
    -- 1. Available (Đang hoạt động))
    @Status TINYINT,
	@IsDefault BIT,
	@GenderId INT, 
    @DoB DATE,
	@Tag VARCHAR(120) = '',
	@CreditLimit NUMERIC(16, 2),
	@PaymentTerm NUMERIC(5, 0),
	@StockId INT,
	@Representative NVARCHAR(32),
	@RepPhone VARCHAR(12),
	@Operator NVARCHAR(32),
	@OpePhone VARCHAR(12),
	@Map VARCHAR(1000),
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check Customer is have default
			IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Customer] WHERE [IsDefault] = 1) 
				SET @IsDefault = 1

			-- Set Default
			IF(@IsDefault = 1)
				UPDATE [dbo].[Customer] SET [IsDefault] = 0 WHERE [IsDefault] = 1

			IF(@PaymentMethodId = 0)
				SELECT TOP 1 @PaymentMethodId = [Id] FROM [dbo].[PaymentMethod]

			--IF(@MembershipClassId = 0)
			--	SELECT TOP 1 @MembershipClassId = [Id] FROM [dbo].[MembershipClass] WHERE [IsDefault] = 1

			IF(@Tag IS NULL)
				SET @Tag = @Phone

			IF @GenderId = 0
				SET @GenderId = NULL

			INSERT INTO [dbo].[Customer]
					([Name], [IdentityCode], [Type], 
					 [Phone], [Email],[Address], 
					 [PaymentMethodId], [CustomerGroupId],
					 [IsInvoice], [Note], [Status], [IsDefault], [GenderId], [DoB], [Tag],[CreateDate], [UserId])

			VALUES  (@Name , @IdentityCode , @Type , 
					 @Phone , @Email , @Address, 
					 @PaymentMethodId , @CustomerGroupId,
					 @IsInvoice , @Note , @Status , @IsDefault , @GenderId , @DoB, @Tag, CAST(GETDATE() AS DATE), @UserId)

			SET @Id = SCOPE_IDENTITY()

		COMMIT
		SELECT C.* 
		FROM [dbo].[Customer] AS C
		WHERE C.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
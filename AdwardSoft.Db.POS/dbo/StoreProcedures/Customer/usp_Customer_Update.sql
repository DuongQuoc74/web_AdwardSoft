CREATE PROCEDURE [dbo].[usp_Customer_Update]
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
    @IsInvoice BIT, 
    @Note NVARCHAR(100),
    -- 0. Unavailable (Ngừng hoạt động) 
    -- 1. Available (Đang hoạt động))
    @Status TINYINT,
	@IsDefault BIT,
	@GenderId INT, 
    @DoB DATE,
	@Tag VARCHAR(120),
	@CreditLimit NUMERIC(16, 2),
	@PaymentTerm NUMERIC(5, 0),
	@StockId INT,
	@Representative NVARCHAR(32),
	@RepPhone VARCHAR(12),
	@Operator NVARCHAR(32),
	@OpePhone VARCHAR(12),
	@Map VARCHAR(1000)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check is default
			DECLARE @Defaulting BIT;

			SELECT TOP 1 @Defaulting = [IsDefault]
			FROM [dbo].[Customer]
			WHERE [Id] = @Id

			-- if Customer is not default 
			-- want to set default
			IF(@IsDefault = 1 AND @Defaulting = 0)
			BEGIN
				DECLARE @DefaultingId INT;

				SELECT TOP 1 @DefaultingId = [Id]
				FROM [dbo].[Customer]
				WHERE [IsDefault] = 1

				--> Update default = 0
				UPDATE [dbo].[Customer] 
				SET [IsDefault] = 0 
				WHERE [Id] = @DefaultingId
			END

			-- if stock is defaulting
			-- want to not set default
			IF(@IsDefault = 0 AND @Defaulting = 1)
			BEGIN
				DECLARE @newDefault INT = 0;

				SELECT TOP 1 @newDefault = [Id]		
				FROM [dbo].[Customer] 
				WHERE [IsDefault] = 0

				UPDATE [dbo].[Customer] 
				SET [IsDefault] = 1
				WHERE [Id] = @newDefault
			END

			IF @GenderId = 0
				SET @GenderId = NULL

			IF @StockId = 0
				SET @StockId = NULL

			UPDATE [dbo].[Customer]
			SET [Name] = @Name, [IdentityCode] = @IdentityCode, [Type] = @Type, 
				[Phone] = @Phone, [Email] = @Email, [Address] = @Address, 
				[PaymentMethodId] = @PaymentMethodId, [CustomerGroupId] = @CustomerGroupId,
				[IsInvoice] = @IsInvoice, [Note] = @Note,
				[Status] = @Status, [IsDefault] = @IsDefault, [GenderId] = @GenderId, [DoB] = @DoB,
				[Tag] = @Tag, [CreditLimit] = @CreditLimit, [PaymentTerm] = @PaymentTerm, [StockId] = @StockId, [Representative] = @Representative, [RepPhone] = @RepPhone, [Operator] = @Operator,[OpePhone] = @OpePhone, [Map] = @Map
			WHERE [Id] = @Id

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
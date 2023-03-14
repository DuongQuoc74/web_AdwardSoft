CREATE PROCEDURE [dbo].[usp_CustomerWallet_Create]
	@Id CHAR(13), 
    @Date DATETIME, 
	@Note NVARCHAR(300),
	@BankAccountId INT = 0,
	@Amount NUMERIC(16, 2),
	@Type TINYINT = 0,
    @CustomerId INT, 
    @Status TINYINT = 0
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			
            SET @Id = REPLACE(NEWID(), '-','')
			DECLARE @Check TINYINT = (SELECT [dbo].[fn_CheckTopupCredit](@Id, @CustomerId, @Type, @Amount))

			--Nap the thanh toan
			IF @BankAccountId > 0 
			BEGIN
				INSERT INTO [dbo].[CustomerWallet] ([Id], [Date], [Note], [Amount], [BankAccountId], [Type],
													[CustomerId], [Status])
				VALUES (@Id, @Date, @Note, @Amount, @BankAccountId, @Type, @CustomerId, @Status)
			END
			-- Thanh Toán Đơn Đặt hàng
			ELSE IF @BankAccountId = 0
				INSERT INTO [dbo].[CustomerWallet] ([Id], [Date], [Note], [Amount], [BankAccountId], [Type],
													[CustomerId], [Status])
				VALUES (@Id, @Date, @Note, @Amount, @BankAccountId, @Type, @CustomerId, @Status)
			--Cap tin dung
			ELSE IF @Check = 1
			BEGIN
				INSERT INTO [dbo].[CustomerWallet] ([Id], [Date], [Note], [Amount], [BankAccountId], [Type],
													[CustomerId], [Status])
				VALUES (@Id, @Date, @Note, @Amount, @BankAccountId, @Type, @CustomerId, @Status)
			END
			ELSE
				SET @Id = NULL
		COMMIT	
		SELECT @Id;
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT '';
		THROW;
	END CATCH
END

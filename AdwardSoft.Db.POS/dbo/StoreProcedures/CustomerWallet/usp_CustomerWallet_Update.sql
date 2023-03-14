CREATE PROCEDURE [dbo].[usp_CustomerWallet_Update]
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
		DECLARE @Check TINYINT = (SELECT [dbo].[fn_CheckTopupCredit](@Id, @CustomerId, @Type, @Amount))
		BEGIN TRAN
			
			IF @BankAccountId > 0
			BEGIN
				UPDATE [dbo].[CustomerWallet] SET [Date] = @Date, 
												  [Note] = @Note, 
												  [Amount] = @Amount, 
												  [BankAccountId] = @BankAccountId
				WHERE [Id] = @Id AND [Status] = 0
			END
			ELSE IF @Check = 1
			BEGIN
				UPDATE [dbo].[CustomerWallet] SET [Date] = @Date, 
												  [Note] = @Note, 
												  [Amount] = @Amount,
												  [CustomerId] = @CustomerId
				WHERE [Id] = @Id AND [Status] = 0
			END
			
		COMMIT	
		IF @Check = 0 AND @BankAccountId = 0
			SELECT NULL
		ELSE
			SELECT C.* 
			FROM [dbo].[CustomerWallet] AS C
			WHERE C.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT '';
		THROW;
	END CATCH
END

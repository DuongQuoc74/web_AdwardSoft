CREATE PROCEDURE [dbo].[usp_CustomerOrder_Update]
	@Id CHAR(32),
	@PaymentDate DATETIME, 
	@PaymentUser BIGINT, 
	--0: Chưa thanh toán, 1: Đã thanh toán, 2: Từ chối
	@PaymentStatus TINYINT
AS

BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- This will update multiple OrderId the same time
			UPDATE [dbo].[CustomerOrder]
			SET [PaymentDate] = @PaymentDate,
				[PaymentUser] = @PaymentUser,
				[PaymentStatus] = @PaymentStatus
			WHERE [Id] = @Id AND [PaymentStatus] != 1

		COMMIT
		RETURN 1;
	END TRY

	BEGIN CATCH
		ROLLBACK;
		RETURN 0;
		THROW;
	END CATCH
END

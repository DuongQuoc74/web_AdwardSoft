CREATE PROCEDURE [dbo].[usp_ReceivingReportStatus_Update]
	@Id CHAR(32),  
    @Status TINYINT, -- 0: Wating - Chờ duyệt, 1: Approved - Đã duyệt, 2 : Trash - Xoá
	@ModifiedUser BIGINT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN	          
				UPDATE  [dbo].[ReceivingReport] 
                 SET    [Status] = @Status,
						[ModifiedUser] = @ModifiedUser,
						[ModifiedDate] = GETDATE()
				WHERE	[Id] = @Id		
		COMMIT
		SELECT 1 
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN;
		SELECT 0;
		THROW;
	END CATCH
END

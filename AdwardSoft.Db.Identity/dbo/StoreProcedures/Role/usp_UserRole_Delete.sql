﻿CREATE PROCEDURE [dbo].[usp_UserRole_Delete]
	@Id BIGINT
AS BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	DECLARE @return BIT = 1
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM [dbo].[UserRole]
			WHERE [UserId] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		SET @return = 0
		ROLLBACK TRAN
		THROW;
	END CATCH
	RETURN @return;
END

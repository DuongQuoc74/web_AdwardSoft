﻿CREATE PROCEDURE [dbo].[usp_Shift_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[Shift]
			WHERE [Id] = @Id

			SELECT 1;
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

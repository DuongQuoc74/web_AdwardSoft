﻿CREATE PROCEDURE [dbo].[usp_ServiceDisplay_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DELETE [dbo].[ServiceDisplay]
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
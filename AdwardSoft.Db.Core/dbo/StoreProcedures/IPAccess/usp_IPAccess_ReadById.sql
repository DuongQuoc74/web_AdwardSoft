﻿CREATE PROCEDURE [dbo].[usp_IPAccess_ReadById]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		BEGIN TRAN;
			SELECT * 
			FROM [dbo].[IPAccess]
			WHERE [Id] = @Id
		COMMIT	
	END TRY
	BEGIN CATCH
		ROLLBACK;
		THROW;
	END CATCH
END

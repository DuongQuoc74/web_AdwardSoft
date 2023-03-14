﻿CREATE PROCEDURE [dbo].[usp_Product_ReadCodeIsUsing]
	@Code VARCHAR(254)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
	BEGIN TRAN
		DECLARE @Return INT = 0;
		DELETE FROM [dbo].[Product]
		WHERE [Code]=@Code
		COMMIT
	END TRY
	BEGIN CATCH
	ROLLBACK TRAN;
		SELECT @Return =1;
		THROW;
	END CATCH
END

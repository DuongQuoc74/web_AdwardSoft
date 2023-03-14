﻿CREATE PROCEDURE [dbo].[usp_Branch_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT b.* 
		FROM [Branch] b
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
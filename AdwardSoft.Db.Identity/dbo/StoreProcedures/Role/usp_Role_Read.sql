﻿CREATE PROCEDURE [dbo].[usp_Role_Read]
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[Role]
		WHERE [Name] NOT LIKE 'Administrator'
		ORDER BY [Name]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
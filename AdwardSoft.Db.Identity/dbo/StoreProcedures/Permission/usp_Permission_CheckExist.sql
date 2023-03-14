﻿CREATE PROCEDURE [dbo].[usp_Permission_CheckExist]
	@Id INT,
	@Controller VARCHAR(50),
	@Action VARCHAR(20)
AS 
	BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF(@Id <> 0)
		BEGIN
			SELECT TOP 1 p.*
			FROM [dbo].[Permission] p		
			WHERE p.[Id] <> @Id AND  p.[Controller] = @Controller AND p.[Action] = @Action
		END
		ELSE
		BEGIN
			SELECT TOP 1 p.*
			FROM [dbo].[Permission] p		
			WHERE p.[Controller] = @Controller AND p.[Action] = @Action
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

﻿CREATE PROCEDURE [dbo].[usp_DeliveryPoint_Delete]
		@Id INT
AS 
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Check INT = 0 
		SELECT TOP 1 @Check = 1 
		FROM [dbo].[CustomerOrder] 
		WHERE [DeliveryPointId] = @Id
		IF @Check = 0
		BEGIN
		DELETE [dbo].[DeliveryPoint] WHERE [Id] = @Id 
				SELECT 1;
				END
			ELSE 
				SELECT 0;

		COMMIT
	END TRY 
	BEGIN CATCH
		ROLLBACK;
		SELECT 0 ;
		THROW;
	END CATCH 
END
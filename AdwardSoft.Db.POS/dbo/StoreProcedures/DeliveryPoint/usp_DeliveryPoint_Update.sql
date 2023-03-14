CREATE PROCEDURE [dbo].[usp_DeliveryPoint_Update]
	@Id INT,
	@Code VARCHAR(6), 
	@Name NVARCHAR(120),
	@Rate NUMERIC(3),
	@LocationId INT,
	@Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[DeliveryPoint]
			SET [Code] = @Code,
				[Name] = @Name,
				[Rate] = @Rate,
				[LocationId] = @LocationId,
				[Status] = @Status
			WHERE [Id] = @Id
		COMMIT

		SELECT L.* 
		FROM [dbo].[DeliveryPoint] AS L
		WHERE L.[Id] = @Id
	END TRY
	BEGIN CATCH
		--ROLLBACK TRAN;
		--RETURN 0;
		--THROW;
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

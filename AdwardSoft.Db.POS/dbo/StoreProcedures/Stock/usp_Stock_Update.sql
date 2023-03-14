CREATE PROCEDURE [dbo].[usp_Stock_Update]
	@Id INT, 
    @Name NVARCHAR(60), 
    @BranchId INT, 
    @Description NVARCHAR(100), 
	-- 1. Inventory Tracking (Theo dõi tồn kho)
    -- 2. No Inventory Tracking (Không theo dõi tồn kho)
    @Type TINYINT,
    @IsDefault BIT,
	@StockIsUsing TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check is default
			DECLARE @Defaulting BIT;

			SELECT TOP 1 @Defaulting = [IsDefault]
			FROM [dbo].[Stock]
			WHERE [Id] = @Id AND [BranchId] = @BranchId

			-- if stock is not default 
			-- want to set default
			IF(@IsDefault = 1 AND @Defaulting = 0)
			BEGIN
				DECLARE @DefaultingId INT;

				SELECT @DefaultingId = [Id]
				FROM [dbo].[Stock]
				WHERE [IsDefault] = 1 AND [BranchId] = @BranchId

				--> Update default = 0
				UPDATE [dbo].[Stock] 
				SET [IsDefault] = 0 
				WHERE [Id] = @DefaultingId AND [BranchId] = @BranchId
			END

			-- if stock is defaulting
			-- want to not set default
			IF(@IsDefault = 0 AND @Defaulting = 1)
			BEGIN
				DECLARE @newDefault INT = 0;

				SELECT TOP 1 @newDefault = [Id]		
				FROM [dbo].[Stock]
				WHERE [IsDefault] = 0 AND [BranchId] = @BranchId

				UPDATE [dbo].[Stock]
				SET [IsDefault] = 1
				WHERE [Id] = @newDefault
			END

			-- update
			UPDATE [dbo].[Stock]
			SET [Name] = @Name, 
				[Description] = @Description, 
				[Type] = @Type, 
				[BranchId] = @BranchId,
				[IsDefault] = @IsDefault
			WHERE [Id] = @Id

		COMMIT
		SELECT S.* 
		FROM [dbo].[Stock] AS S
		WHERE S.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
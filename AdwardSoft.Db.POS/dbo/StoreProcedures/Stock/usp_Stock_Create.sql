CREATE PROCEDURE [dbo].[usp_Stock_Create]
	@Id INT, 
    @Name NVARCHAR(60), 
    @BranchId INT, 
    @Description NVARCHAR(100), 
    @Type TINYINT, -- 0. Theo dỗi tồn kho, 1. Không theo dõi tồn kho
    @IsDefault BIT,
	@StockIsUsing TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			-- Check Stock is have default
			DECLARE @Defaulting BIT = 0

			SELECT TOP 1 @Defaulting = 1 
			FROM [dbo].[Stock] 
			WHERE [IsDefault] = 1 AND [BranchId] = @BranchId

			IF(@Defaulting = 0)
				SET @IsDefault = 1

			-- SET Default
			IF(@IsDefault = 1)
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

			INSERT INTO [dbo].[Stock]
					([Name], [BranchId], [Description], [Type], [IsDefault])
			VALUES  (@Name , @BranchId,  @Description,  @Type,  @IsDefault)

			SET @Id = SCOPE_IDENTITY()

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
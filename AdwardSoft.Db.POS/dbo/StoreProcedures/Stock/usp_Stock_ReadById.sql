CREATE PROCEDURE [dbo].[usp_Stock_ReadById]
	@Id INT
AS
BEGIN
	DECLARE @IsUsing INT = 0
	-- Check stockId is already used by product
	SELECT TOP 1 @IsUsing = 1
	FROM [dbo].[Product] 
	WHERE [StockId] = @Id

	SELECT *, @IsUsing AS StockIsUsing
	FROM [dbo].[Stock]
	WHERE [Id] = @Id
END

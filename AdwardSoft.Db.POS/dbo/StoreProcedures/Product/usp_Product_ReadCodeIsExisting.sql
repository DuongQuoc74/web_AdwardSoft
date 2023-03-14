CREATE PROCEDURE [dbo].[usp_Product_ReadCodeIsExisting]
	@Id INT,
	@Code VARCHAR(254)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0;
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[Product] AS P
		WHERE (P.[Code] = @Code AND P.[Id] <> @Id)
	
		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
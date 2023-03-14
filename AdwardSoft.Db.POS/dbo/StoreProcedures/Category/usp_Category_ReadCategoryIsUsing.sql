CREATE PROCEDURE [dbo].[usp_Category_ReadCategoryIsUsing]
@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT @Return = 1 
		FROM [dbo].[Category] AS S
		INNER JOIN [dbo].[ProductCategory] AS SC ON S.[Id] = SC.[CategoryId]
		WHERE S.[Id] = @Id
		RETURN @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END


CREATE PROCEDURE [dbo].[usp_CustomerGroup_ReadCheckIsUsing]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[CustomerGroup] AS CG
		INNER JOIN [dbo].[Customer] AS C ON CG.[Id] = C.CustomerGroupId
		WHERE CG.[Id] = @Id

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
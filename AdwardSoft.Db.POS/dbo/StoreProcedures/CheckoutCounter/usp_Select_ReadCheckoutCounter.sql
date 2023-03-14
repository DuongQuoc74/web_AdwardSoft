CREATE PROCEDURE [dbo].[usp_Select_ReadCheckoutCounter]
	@BranchId INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		IF (@BranchId <> 0)
		BEGIN
			SELECT [Id], [Name] AS [Text]
			FROM [CheckoutCounter] 
			WHERE [BranchId] = @BranchId
		END
		ELSE
		BEGIN
			SELECT [Id], [Name] AS [Text]
			FROM [CheckoutCounter] 
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

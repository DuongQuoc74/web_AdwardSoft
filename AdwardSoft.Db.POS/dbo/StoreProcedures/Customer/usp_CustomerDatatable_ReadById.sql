CREATE PROCEDURE [dbo].[usp_CustomerDatatable_ReadById]
	@Id INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		SELECT C.*, CG.[Name] AS [CustomerGroupName], CG.[PricingPolicy] AS [PricingPolicy],
						ISNULL(G.[Name], '') AS [GenderName], P.[Name] AS [PaymentMethodName], ISNULL(S.[Name], '') AS [StockName]
						FROM [dbo].[Customer] AS C
						INNER JOIN [dbo].[CustomerGroup] AS CG ON C.[CustomerGroupId] = CG.[Id]
						LEFT JOIN [dbo].[Gender] AS G ON C.[GenderId] IS NOT NULL AND C.[GenderId] = G.[Id]
						INNER JOIN [dbo].[PaymentMethod] as P ON C.[PaymentMethodId] = P.[Id] 
						LEFT JOIN [dbo].[Stock] AS S ON C.[StockId] IS NOT NULL AND C.[StockId] = S.[Id] 
						WHERE C.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

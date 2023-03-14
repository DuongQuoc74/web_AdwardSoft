CREATE FUNCTION [dbo].[fn_ExportStock]
(
  @Id INT,
  @DateFrom DATE,
  @DateTo DATE,
  @BranchId INT
)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
 DECLARE @s INT;

 SELECT @s = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[SaleReceipt] R
 INNER JOIN [dbo].[SaleReceiptDetail] RD ON R.[Id] = RD.[SaleReceiptId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 0 AND R.[BranchId] = @BranchId
 AND @DateFrom <=  CONVERT(DATE, R.[CreateDate]) AND  @DateTo >=  CONVERT(DATE, R.[CreateDate]) 

 RETURN (ISNULL(@s,0));
END

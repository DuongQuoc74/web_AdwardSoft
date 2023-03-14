CREATE FUNCTION [dbo].[fn_BeginingStock]
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
 DECLARE @s1 INT;
 DECLARE @s2 INT;
 DECLARE @s3 INT;
 
 SELECT @s = SUM(b.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[BeginingStock] B
 INNER JOIN [dbo].[Stock] R ON R.[Id] = B.[StockId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = B.[ProductId] AND PU.[UnitId] = B.[UnitId]
 WHERE B.[ProductId] = @Id AND B.[Year] = YEAR(@DateFrom) AND R.[BranchId] = @BranchId

 SELECT @s1 = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[ReceivingReport] R
 INNER JOIN [dbo].[ReceivingReportDetail] RD ON R.[Id] = RD.[ReceivingReportId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 1 AND R.[BranchId] = @BranchId
 AND YEAR(@DateFrom) = YEAR(R.[CreateDate]) AND  @DateFrom >= CONVERT(DATE, R.[CreateDate]) 
 
 SELECT @s2 = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[SaleReturn] R
 INNER JOIN [dbo].[SaleReturnDetail] RD ON R.[Id] = RD.[SaleReturnId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 0 AND R.[BranchId] = @BranchId
 AND YEAR(@DateFrom) = YEAR(R.[CreateDate]) AND  @DateFrom >= CONVERT(DATE, R.[CreateDate])

 SELECT @s3 = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[SaleReceipt] R
 INNER JOIN [dbo].[SaleReceiptDetail] RD ON R.[Id] = RD.[SaleReceiptId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 0 AND R.[BranchId] = @BranchId
 AND YEAR(@DateFrom) = YEAR(R.[CreateDate]) AND  @DateFrom >= CONVERT(DATE, R.[CreateDate]) 

  SET @s = ISNULL(@s, 0) + ISNULL(@s1, 0) + ISNULL(@s2, 0) - ISNULL(@s3, 0)

 RETURN (ISNULL(@s,0));
END

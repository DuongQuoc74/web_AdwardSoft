CREATE FUNCTION [dbo].[fn_ImportStock]
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

 SELECT @s1 = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[ReceivingReport] R
 INNER JOIN [dbo].[ReceivingReportDetail] RD ON R.[Id] = RD.[ReceivingReportId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 1 AND R.[BranchId] = @BranchId
 AND @DateFrom <=  CONVERT(DATE, R.[CreateDate]) AND  @DateTo >=  CONVERT(DATE, R.[CreateDate]) 
 
 SELECT @s2 = SUM(RD.[Quantity]*(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[SaleReturn] R
 INNER JOIN [dbo].[SaleReturnDetail] RD ON R.[Id] = RD.[SaleReturnId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 0 AND R.[BranchId] = @BranchId
 AND @DateFrom <=  CONVERT(DATE, R.[CreateDate]) AND  @DateTo >=  CONVERT(DATE, R.[CreateDate])

 SET @s = ISNULL(@s1, 0) + ISNULL(@s2, 0)

 RETURN (ISNULL(@s,0));
END

CREATE FUNCTION [dbo].[fn_AvgPrice]
(
  @Id INT,
  @SupplierId INT,
  @BranchId INT
)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
 DECLARE @s INT;
 
 SELECT @s = AVG(RD.[Price]/(ISNULL(PU.[QuantityTo], 1)))
 FROM [dbo].[ReceivingReport] R
 INNER JOIN [dbo].[ReceivingReportDetail] RD ON R.[Id] = RD.[ReceivingReportId]
 LEFT JOIN [dbo].[ProductUnitConverter] PU ON PU.[ProductId] = RD.[ProductId] AND PU.[UnitId] = RD.[UnitId]
 WHERE RD.[ProductId] = @Id AND R.[Status] = 1 AND R.[SupplierId] = @SupplierId AND R.[BranchId] = @BranchId

 RETURN (ISNULL(@s,0));
END


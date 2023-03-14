CREATE FUNCTION [dbo].[fn_ReceiptCalc]
(
  @Code CHAR(2)
)
RETURNS CHAR(20)
WITH SCHEMABINDING
AS
BEGIN
 
 DECLARE @NoC INT;
 DECLARE @Prefix VARCHAR(10);
 DECLARE @Suffix VARCHAR(10);
 DECLARE @CurrentIdx INT;
 DECLARE @VoucherNo CHAR(20);
 DECLARE @Format VARCHAR(10);

 SELECT @Noc = [NoC], @Prefix = [Prefix], @Suffix = [Suffix], @CurrentIdx = (ISNULL([CurrentIdx] + 1, [StartNo]))
 FROM [dbo].[ReceiptSetting]
 WHERE [Code] = @Code

 SET @Format = 'd' + CAST(@NoC AS VARCHAR)

 SET @VoucherNo = @Prefix + FORMAT(@CurrentIdx, @Format) + @Suffix
 

 RETURN (ISNULL(@VoucherNo,''));
END


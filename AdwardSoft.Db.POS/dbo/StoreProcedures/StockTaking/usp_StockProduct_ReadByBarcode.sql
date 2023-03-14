CREATE PROCEDURE [dbo].[usp_StockProduct_ReadByBarcode]
	@Barcode CHAR(13)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @CountBar INT,@ProductId INT
		SELECT @CountBar=COUNT(*),@ProductId=Id FROM [dbo].[Product] WHERE Code=@Barcode GROUP BY Id;
		IF(@CountBar>0)
		begin
			--UPDATE [dbo].[Stocktaking]
			--SET Quantity=Quantity+1
			--WHERE ProductId=@ProductId;

			SELECT P.[Id],P.[Name] AS ProductName,P.[Image],S.[Id] AS StockId,S.[Name] AS StockName, ST.[Date], ST.[Quantity]
			FROM [dbo].[Product] P, [dbo].[Stocktaking] ST, [dbo].[Stock] S
			WHERE P.[Id]=ST.[ProductId] AND S.[Id]=ST.[StockId] AND P.Code=@Barcode
		end
		ELSE
			SELECT 0;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

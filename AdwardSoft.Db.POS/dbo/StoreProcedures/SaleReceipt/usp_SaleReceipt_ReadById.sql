CREATE PROCEDURE [dbo].[usp_SaleReceipt_ReadById]
	@Id CHAR(32)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT s.* 
		FROM [SaleReceipt] s
		WHERE s.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

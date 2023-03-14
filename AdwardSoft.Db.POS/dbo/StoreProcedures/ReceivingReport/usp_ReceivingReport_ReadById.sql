CREATE PROCEDURE [dbo].[usp_ReceivingReport_ReadById]
	@Id CHAR(32)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		SELECT *
		FROM [dbo].[ReceivingReport] 
		WHERE [Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

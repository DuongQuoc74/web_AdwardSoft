CREATE PROCEDURE [dbo].[usp_Supplier_ReadPhoneIsExiting]
	@Id INT,
	@Tel VARCHAR(12)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[Supplier] AS S
		WHERE S.[Tel] = @Tel AND S.[Id] <> @Id

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

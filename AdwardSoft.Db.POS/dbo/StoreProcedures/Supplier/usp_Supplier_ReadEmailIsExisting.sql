CREATE PROCEDURE [dbo].[usp_Supplier_ReadEmailIsExisting]
	@Id INT,
	@Email VARCHAR(254)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0;
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[Supplier] AS S
		WHERE (S.[Email] = @Email AND S.[Id] <> @Id) AND (@Email IS NOT NULL)

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
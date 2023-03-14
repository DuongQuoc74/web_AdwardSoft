CREATE PROCEDURE [dbo].[usp_EmployeElastic_Read]
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT [Id], [Code], [IdentityCode], [Name]
		FROM [dbo].[Employee]
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

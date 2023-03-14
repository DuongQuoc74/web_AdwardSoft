CREATE PROCEDURE [dbo].[usp_Unit_ReadCheckUnitIsUsing]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT @Return = 1 

		FROM [dbo].[Unit] AS U
		INNER JOIN [dbo].[Product] AS P ON U.[Id] = P.[UnitId]
		WHERE U.[Id] = @Id

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
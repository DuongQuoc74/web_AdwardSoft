CREATE PROCEDURE [dbo].[usp_MembershipClass_ReadCheckIsUsing]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0

		SELECT TOP 1 @Return = 1
		FROM [dbo].[MembershipClass] AS M
		--INNER JOIN [dbo].[Customer] AS C ON M.[Id] = C.[MembershipClassId]
		WHERE M.[Id] = @Id


		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
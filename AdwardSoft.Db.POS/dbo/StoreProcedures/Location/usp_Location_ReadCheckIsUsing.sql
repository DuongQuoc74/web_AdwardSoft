CREATE PROCEDURE [dbo].[usp_Location_ReadCheckIsUsing]
	@Id INT
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
	
		SELECT TOP 1 @Return = 1 
		FROM [dbo].[DeliveryPoint] AS DP
		WHERE DP.[LocationId] = @Id

		SELECT TOP 1 @Return = 1 
		FROM [dbo].[Location] AS L
		WHERE L.[ParentId] = @Id AND L.[Id] != L.[ParentId]

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
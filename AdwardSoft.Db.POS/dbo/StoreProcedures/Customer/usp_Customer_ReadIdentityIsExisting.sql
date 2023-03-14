CREATE PROCEDURE [dbo].[usp_Customer_ReadIdentityIsExisting]
	@Id INT,
	@Identity VARCHAR(20)
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return INT = 0
		IF(LEN(@Identity) > 0)
			SELECT TOP 1 @Return = 1 
			FROM [dbo].[Customer] AS C
			WHERE C.[IdentityCode] = @Identity AND C.[Id] <> @Id

		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_Promotion_ReadById]
	@Id int 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT p.* 
		FROM [dbo].[Promotion] p
		WHERE p.[Id] = @Id
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

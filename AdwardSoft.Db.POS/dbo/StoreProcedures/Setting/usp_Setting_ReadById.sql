CREATE PROCEDURE [dbo].[usp_Setting_ReadById]
	@Id int 
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		SELECT TOP 1 p.* 
		FROM [dbo].[Setting] p
		WHERE p.[Id] = @Id OR @Id = 0
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

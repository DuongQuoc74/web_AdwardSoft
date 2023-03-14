CREATE PROCEDURE [dbo].[usp_Unit_ReadCheckCodeIsExisting]
	@Id INT = 0,
	@Code CHAR(10) = ''
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '@Id INT, 
											  @Code CHAR(10) '
		
		SET @SqlStr = N'SELECT U.* 
						FROM [dbo].[Unit] AS U 
						WHERE 1 = 1 '

		IF(@Id <> 0)
			SET @SqlStr = @SqlStr + N' AND U.[Id] <> @Id '

		IF(@Code <> '')
			SET @SqlStr = @SqlStr + N' AND U.[Code] = @Code '


		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @Id,
						   @Code
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_MembershipClass_Read]
	--@Id INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		--DECLARE @SqlStr NVARCHAR(MAX),
		--		@ParamList  NVARCHAR(2000) = '@Id INT '
		
		--SET @SqlStr = N'SELECT M.* 
		--				FROM [dbo].[MembershipClass] AS M
		--				WHERE 1 = 1 '

		--IF(@Id <> 0)
		--	SET @SqlStr = @SqlStr + N' AND M.[Id] = @Id '


		--EXEC SP_EXECUTESQL @SqlStr, 
		--				   @ParamList,
		--				   @Id

		SELECT * 
			FROM [dbo].[MembershipClass]
			ORDER BY [Level]
						   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
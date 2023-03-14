CREATE PROCEDURE [dbo].[usp_ScoreConfiguration_Read]
@Id INT=0
AS
BEGIN
	SET NOCOUNT OFF;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@ParamList  NVARCHAR(2000) = '@Id INT'
			
		SET @SqlStr = N'SELECT S.* 
					    FROM [dbo].[ScoreConfiguration] AS S
						WHERE 1 = 1 '
		IF(@Id <> 0)
			SET @SqlStr = @SqlStr + N' AND S.[Id] = @Id '

			SET @SqlStr=@SqlStr+' ORDER BY [EffectiveDate] DESC'
		EXEC SP_EXECUTESQL @SqlStr, 
						   @ParamList,
						   @Id		   
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
CREATE PROCEDURE [dbo].[usp_HandleError_ReadById]
	@Id INT,
	@LanguageCode CHAR(2)
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		SELECT * 
		FROM [dbo].[HandleError] h
		INNER JOIN [dbo].[HandleErrorTrans] ht ON ht.[HandleErrorId] = h.[Id]
		WHERE h.[Id] = @Id AND ht.[LanguageCode] = @LanguageCode
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

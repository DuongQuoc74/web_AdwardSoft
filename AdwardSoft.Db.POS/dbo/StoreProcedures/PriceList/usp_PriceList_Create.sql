CREATE PROCEDURE [dbo].[usp_PriceList_Create]
	@Date DATE,
    @Title NVARCHAR(150),
    @Note NVARCHAR(300),
	@Status TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[PriceList]
					([Date], [Title], [Note], [Status])
			VALUES  (@Date, @Title, @Note, @Status)
		COMMIT
		SELECT P.* 
		FROM [dbo].[PriceList] AS P
		WHERE P.[Date] = @Date
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END


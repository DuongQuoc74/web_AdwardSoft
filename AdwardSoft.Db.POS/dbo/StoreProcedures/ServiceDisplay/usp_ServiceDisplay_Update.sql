CREATE PROCEDURE [dbo].[usp_ServiceDisplay_Update]
	@Id INT, 
    @Name NVARCHAR(120), 
    @SupplierId INT, 
    @Date DATE, 
    @DateFrom DATE, 
    @DateTo DATE, 
    @Description NVARCHAR(300), 
    @Fee NUMERIC, 
    @PaymentPeriod TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			UPDATE [dbo].[ServiceDisplay]
			SET	[Name] = @Name,
				[SupplierId] = @SupplierId,  
				[DateFrom] = @DateFrom, 
				[DateTo] = @DateTo, 
				[Description] = @Description, 
				[Fee] = @Fee, 
				[PaymentPeriod] = @PaymentPeriod
			WHERE [Id] = @Id 
		COMMIT
		SELECT G.* 
		FROM [dbo].[Gender] AS G
		WHERE G.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

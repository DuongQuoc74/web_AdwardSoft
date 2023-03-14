CREATE PROCEDURE [dbo].[usp_ServiceDisplay_Create]
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
			INSERT INTO [dbo].[ServiceDisplay]
					([Name],
					 [SupplierId], 
					 [Date], 
					 [DateFrom], 
					 [DateTo], 
					 [Description], 
					 [Fee], 
					 [PaymentPeriod])
			VALUES  (@Name,
					 @SupplierId, 
					 GETDATE(), 
					 @DateFrom, 
					 @DateTo, 
					 @Description, 
					 @Fee, 
					 @PaymentPeriod)
			SET @Id = SCOPE_IDENTITY()
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

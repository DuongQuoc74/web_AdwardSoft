CREATE PROCEDURE [dbo].[usp_MembershipScore_Create]
	@CustomerId INT, 
    @SaleReceiptId CHAR(32), 
    @Year CHAR(4), 
    @Type TINYINT = 1, 
    @Amount NUMERIC(16,2), 
    @Point NUMERIC(9,0)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			DECLARE @AmountConfig NUMERIC(16,2)
			DECLARE @CalcAmount NUMERIC(16,2)= @Amount
			DECLARE @PointConfig NUMERIC(9,0)= 0

			SET @Year = CAST(YEAR(GETDATE()) AS CHAR(4))
			
			IF(@Type = 1 OR @Type = 2)
			BEGIN
				-- 0: Exchange, 1: Earn, 2: Return
				--Example: 200,000đ = 1 point
				SELECT TOP 1 @PointConfig = [FromPoint], @AmountConfig = [FromAmount] FROM dbo.[ScoreConfiguration] 
				WHERE [EffectiveDate] <= GETDATE()
				ORDER BY [EffectiveDate] DESC
			
				--Question: example amount = 970,000đ --> ?points?
				WHILE (@CalcAmount > @AmountConfig)
				BEGIN
					SET @Point = @Point + @PointConfig
					SET @CalcAmount = @CalcAmount - @AmountConfig --Exp: -200K (970K+1, 770K+1, 570K+1, 370K+1, 170+0)
				END

				IF(@Type = 2)
				BEGIN
					SET @Point = @Point * -1
					SET @Amount = @Amount *-1
				END
			END

			INSERT INTO [dbo].[MembershipScore]
					([CustomerId], [SaleReceiptId], [Year], [Type], [Amount], [Point])
			VALUES  (@CustomerId, @SaleReceiptId, @Year, @Type, @Amount, @Point)
			
			SELECT 1; --True
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0; --False
		THROW;
	END CATCH
END
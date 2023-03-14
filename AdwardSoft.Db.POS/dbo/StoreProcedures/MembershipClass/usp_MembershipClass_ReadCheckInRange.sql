CREATE PROCEDURE [dbo].[usp_MembershipClass_ReadCheckInRange]
	@Id INT,
	@LowestValue NUMERIC(16, 2),
	@HighestValue NUMERIC(16, 2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @Return BIT = 0

		SELECT TOP 1 @Return = 1
		FROM [dbo].[MembershipClass] 
		WHERE [Id] <> @Id AND
			(
			  ([HighestValue] = 0 AND @HighestValue <> 0 AND @HighestValue > [LowestValue]) 
		      OR 
			  (
				  [HighestValue] <> 0 AND 
				  (
					  ([LowestValue] < @LowestValue AND [HighestValue] > @LowestValue) 
					  OR
					  ([LowestValue] < @HighestValue AND [HighestValue] > @HighestValue)
					  OR
					  ([LowestValue] > @LowestValue AND [HighestValue] < @HighestValue) 
				  )
			  )
			  OR
			  (
				   @HighestValue = 0 AND
				   (
						@LowestValue < [HighestValue]
						OR
						[HighestValue] = 0
				   )
			  )
			)
	
		SELECT @Return
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

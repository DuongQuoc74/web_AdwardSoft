CREATE PROCEDURE [dbo].[usp_Shift_Create]
	@Id int,
	@Name nvarchar(30),
	@IsMonday bit,
	@IsTuesday bit,
	@IsWednesday bit,
	@IsThursday bit,
	@IsFriday bit,
	@IsSaturday bit,
	@IsSunday bit,
	@StartTime char(5),
	@EndTime char(5),
	@BranchId int
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			
			INSERT INTO [dbo].[Shift]
				  ([Name], [IsMonday], [IsTuesday], [IsWednesday], [IsThursday], [IsFriday], [IsSaturday], [IsSunday], [StartTime], [EndTime], [BranchId])
			VALUES(@Name , @IsMonday , @IsTuesday , @IsWednesday , @IsThursday , @IsFriday , @IsSaturday , @IsSunday , @StartTime , @EndTime , @BranchId)

			SET @Id = SCOPE_IDENTITY()

		COMMIT
		SELECT *  
		FROM [dbo].[Shift]
		WHERE [Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
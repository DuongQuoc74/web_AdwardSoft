CREATE PROCEDURE [dbo].[usp_Shift_Update]
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
			UPDATE [dbo].[Shift]
			SET [Name] = @Name, 
				[IsMonday] = @IsMonday,
				[IsTuesday] = @IsTuesday,
				[IsWednesday] = @IsWednesday,
				[IsThursday] = @IsThursday,
				[IsFriday] = @IsFriday,
				[IsSaturday] = @IsSaturday,
				[IsSunday] = @IsSunday,
				[StartTime] = @StartTime,
				[EndTime] = @EndTime,
				[BranchId] = @BranchId
			WHERE [Id] = @Id

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
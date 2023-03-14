CREATE PROCEDURE [dbo].[usp_Employee_Create]
	@Id INT, 
    @Name NVARCHAR(70), 
    @Address NVARCHAR(128), 
    @Phone VARCHAR(20), 
    @IdentityCode VARCHAR(20),
    @Email VARCHAR(256), 
    @GenderId INT, 
    @DepartmentId INT, 
    @PositionId INT, 
    @DoB DATE, 
    @Avatar VARCHAR(2048), 
    @Code VARCHAR(10), 
    @PlaceOfBirth NVARCHAR(150), 
    @Nationality NVARCHAR(20), 
    @Religious TINYINT, -- 0 : Không, 1 : Phật giáo, 2 : Công giáo, 3 : Khác 
    @PermanentAddress NVARCHAR(150), 
    @CurrentAddress NVARCHAR(150), 
    @LeavingDate DATE, 
    @StartingDate DATE,
    @Status TINYINT -- 0 : Không, 1 : nghỉ
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[Employee]
				   ([Name], 
                    [Address], 
                    [Phone], 
                    [IdentityCode],
                    [Email], 
                    [GenderId], 
                    [DepartmentId], 
                    [PositionId], 
                    [DoB], 
                    [Avatar], 
                    [Code], 
                    [PlaceOfBirth], 
                    [Nationality], 
                    [Religious],
                    [PermanentAddress], 
                    [CurrentAddress], 
                    [LeavingDate], 
                    [StartingDate],
                    [Status])

			VALUES (@Name, 
                    @Address, 
                    @Phone, 
                    @IdentityCode,
                    @Email, 
                    @GenderId, 
                    @DepartmentId, 
                    @PositionId, 
                    @DoB, 
                    @Avatar, 
                    @Code, 
                    @PlaceOfBirth, 
                    @Nationality, 
                    @Religious,
                    @PermanentAddress, 
                    @CurrentAddress, 
                    @LeavingDate, 
                    @StartingDate,
                    @Status)

			SET @Id = SCOPE_IDENTITY()
		COMMIT
		SELECT e.* 
		FROM [dbo].[Employee] AS e
		WHERE e.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

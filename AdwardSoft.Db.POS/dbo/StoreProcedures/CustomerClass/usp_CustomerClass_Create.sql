CREATE PROCEDURE [dbo].[usp_CustomerClass_Create]
	@CustomerId INT, 
    @MembershipClassId INT, 
    @Date DATETIME, 
    @OldMembershipClassId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO [dbo].[CustomerClass]
					([CustomerId], [MembershipClassId], [Date], [OldMembershipClassId])
			VALUES  (@CustomerId , @MembershipClassId , @Date , @OldMembershipClassId)

		COMMIT
		SELECT CC.*  
		FROM [dbo].[CustomerClass] AS CC
		WHERE CC.[CustomerId] = @CustomerId AND CC.[MembershipClassId] = @MembershipClassId
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END
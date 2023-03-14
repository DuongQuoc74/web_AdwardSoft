CREATE PROCEDURE [dbo].[usp_Select_ReadEmployeeUser]
	@BranchId INT
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
			SELECT B.[UserId] AS [Id], E.[Name] AS [Text]
			FROM [Employee] E
			INNER JOIN [EmployeeUser] U ON U.[EmployeeId] = E.[Id]
			INNER JOIN [UserBranch] B ON U.[UserId] = B.[UserId]
			WHERE B.[BranchId] = @BranchId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

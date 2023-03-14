CREATE PROCEDURE [dbo].[usp_Select_ReadEmployee]
	@BranchId INT
AS 
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		IF (@BranchId <> 0)
		BEGIN
			SELECT E.[Id], E.[Name] AS [Text]
			FROM [Employee] E
			INNER JOIN [EmployeeUser] U ON U.[EmployeeId] = E.[Id]
			INNER JOIN [UserBranch] B ON U.[UserId] = B.[UserId]
			WHERE B.[BranchId] = @BranchId
		END
		ELSE
		BEGIN
			SELECT [Id], [Name] AS [Text]
			FROM [Employee] 
		END
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END

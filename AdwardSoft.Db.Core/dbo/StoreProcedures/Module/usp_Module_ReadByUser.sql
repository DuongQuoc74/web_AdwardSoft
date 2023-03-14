CREATE PROCEDURE [dbo].[usp_Module_ReadByUser]
	@UserId BIGINT
AS BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		-- Check administrtor role
		IF EXISTS(SELECT TOP 1 1 
				  FROM [AdsDL.Identity].[dbo].[UserRole] ur
				  INNER JOIN [AdsDL.Identity].[dbo].[Role] r ON ur.[RoleId] = r.[Id]
				  WHERE ur.[UserId] = @UserId AND r.[Name] LIKE 'Administrator%')
			BEGIN	
				SELECT		*
				FROM	[Module]
				ORDER BY Sort DESC
			END
		ELSE
			BEGIN
				SELECT		*
				FROM	[Module]
				WHERE	[Id] = [ParentId] OR [Id] IN 
				(		
						SELECT DISTINCT m.[Id]
						FROM		[Module] m
						LEFT JOIN [ModuleType] mt ON mt.ModuleId = m.Id
						INNER JOIN	[AdsDL.Identity].[dbo].[Permission] p ON m.[ControllerName] = p.[Controller]
						INNER JOIN	[AdsDL.Identity].[dbo].[RolePermission] rp ON rp.[PermissionId] = p.[Id]
						INNER JOIN	[AdsDL.Identity].[dbo].[UserRole] ur ON ur.[RoleId] = rp.[RoleId]
						INNER JOIN  [AdsDL.Identity].[dbo].[Role] r ON r.[Id] = ur.[RoleId]
						WHERE		ur.[UserId] = @UserId AND p.[Action] like '%Access%' AND (m.IsPublic = 1 OR mt.ModuleId = r.[UserType])
						
				)
				ORDER BY Sort DESC
			END

		DROP TABLE IF EXISTS #UserRole
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
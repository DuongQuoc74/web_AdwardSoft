CREATE PROCEDURE [dbo].[usp_Position_Read]
	@pageSize INT,
	@skip INT,
	@searchBy NVARCHAR(50),
	@departmentId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	--------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr1 NVARCHAR(MAX), 
				@ParamList  NVARCHAR(2000) = '@pageSize INT,
											  @skip INT,
											  @searchBy NVARCHAR(50),
											  @departmentId INT '
		
		SET @SqlStr1 = N'SELECT p.*, pt.*, dt.[Title] AS [DepartmentTitle],COUNT (*) OVER() AS [Count]
						FROM [Position] p
						INNER JOIN [PositionTrans] pt ON p.[Id] = pt.[PositionId]	
						INNER JOIN [Department] d ON p.[DepartmentId] = d.[Id]
						INNER JOIN [DepartmentTrans] dt ON dt.[DepartmentId] = d.[Id]
						INNER JOIN [Language] l ON pt.[LanguageCode] = l.[Code] AND dt.[LanguageCode] = l.[Code]
						WHERE l.[IsDefault] = 1 AND 1 = 1 ';

		IF(@departmentId <> 0)
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND p.DepartmentId = @departmentId '
		END

		IF(@searchBy <> 'NULL')
		BEGIN
			SET @SqlStr1 = @SqlStr1 + N'AND pt.[Title] LIKE ''%''+@searchBy+''%'' '
		END

		SET @SqlStr1 = @SqlStr1 + N' ORDER BY p.[Sort]	
									 OFFSET @skip ROWS 
									 FETCH NEXT @pageSize ROWS ONLY; '
		EXEC SP_EXECUTESQL @SqlStr1, 
						   @ParamList,
						   @pageSize,
						   @skip,
						   @searchBy,
						   @departmentId
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
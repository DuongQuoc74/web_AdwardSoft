create procedure [dbo].[usp_ProductDatatable_ReadByUnitId]
@UnitId int
AS
BEGIN
	IF @UnitId != 0
		SELECT P.*, u.[Name] AS [UnitName]
			FROM [dbo].[Product] AS P INNER JOIN [dbo].[Unit] AS u ON P.[UnitId] = u.[Id]
			WHERE P.[UnitId] = @UnitId 
		
	ELSE
		SELECT * FROM [dbo].[Product] WHERE [Id] = [UnitId]
END

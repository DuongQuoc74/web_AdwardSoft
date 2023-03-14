CREATE PROCEDURE [dbo].[usp_PriceDetailDatatable_Read]
	@ProductId INT = 0,
	@LocationId INT = 0,
	@LocationChildId INT = 0,
	@DeliveryPointId INT = 0,
	@DeliveryType TINYINT,
	@Date DATE
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	----------------------------------------------------------------------------------------------------------
	BEGIN TRY
		DECLARE @SqlStr NVARCHAR(MAX),
				@SqlInitData NVARCHAR(MAX),
				@ParamList NVARCHAR(2000) = ' @ProductId INT,
											  @LocationId INT,
											  @LocationChildId INT,
											  @DeliveryPointId INT,
											  @DeliveryType TINYINT,
											  @Date DATE '
		SET @SqlInitData = N' (SELECT U.[Name] AS UnitName, P.[Id] AS ProductId, P.[Name] AS ProductName, L.[Id] AS LocationId, L.[ParentId] AS LocationParentId, L.[Name] AS LocationName, D.[Id] AS DeliveryPointId, D.[Name] AS DeliveryPointName, 0 AS DeliveryType
							FROM [dbo].[Product] AS P, [dbo].[Location] AS L, [dbo].[DeliveryPoint] AS D, [dbo].[Unit] AS U
							WHERE L.[Id] != L.[ParentId] AND U.[Id] = P.[UnitId]
							UNION
							SELECT U.[Name] AS UnitName, P.[Id] AS ProductId, P.[Name] AS ProductName, L.[Id] AS LocationId, L.[ParentId] AS LocationParentId, L.[Name] AS LocationName, D.[Id] AS DeliveryPointId, D.[Name] AS DeliveryPointName, 1 AS DeliveryType
							FROM [dbo].[Product] AS P, [dbo].[Location] AS L, [dbo].[DeliveryPoint] AS D, [dbo].[Unit] AS U
							WHERE L.[Id] != L.[ParentId] AND U.[Id] = P.[UnitId]
							UNION
							SELECT U.[Name] AS UnitName, P.[Id] AS ProductId, P.[Name] AS ProductName, L.[Id] AS LocationId, L.[ParentId] AS LocationParentId, L.[Name] AS LocationName, D.[Id] AS DeliveryPointId, D.[Name] AS DeliveryPointName, 2 AS DeliveryType
							FROM [dbo].[Product] AS P, [dbo].[Location] AS L, [dbo].[DeliveryPoint] AS D, [dbo].[Unit] AS U
							WHERE L.[Id] != L.[ParentId] AND U.[Id] = P.[UnitId]
							UNION
							SELECT U.[Name] AS UnitName, P.[Id] AS ProductId, P.[Name] AS ProductName, L.[Id] AS LocationId, L.[ParentId] AS LocationParentId, L.[Name] AS LocationName, D.[Id] AS DeliveryPointId, D.[Name] AS DeliveryPointName, 3 AS DeliveryType
							FROM [dbo].[Product] AS P, [dbo].[Location] AS L, [dbo].[DeliveryPoint] AS D, [dbo].[Unit] AS U
							WHERE L.[Id] != L.[ParentId] AND U.[Id] = P.[UnitId]) AS R '

		SET @SqlStr = N'SELECT R.[UnitName], R.[ProductId], R.[ProductName], R.[LocationId], R.[LocationParentId], R.[DeliveryPointId], R.[DeliveryPointName], R.[DeliveryType], L.[Name] + '
		SET @SqlStr = @SqlStr  + N' '' > '' '
		SET @SqlStr = @SqlStr + N' + R.[LocationName] AS LocationName, CASE WHEN PD.[Price] > 0 THEN PD.[Price] ELSE 0 END AS Price
									FROM '
		SET @SqlStr = @SqlStr + @SqlInitData + N' LEFT JOIN [dbo].[PriceDetail] AS PD ON R.[ProductId] = PD.[ProductId] 
												  AND R.[DeliveryPointId] = PD.[DeliveryPointId]
												  AND R.[LocationId] = PD.[LocationId]
												  AND R.[DeliveryType] = PD.[DeliveryType]
												  AND PD.[Date] = @Date
												  INNER JOIN [dbo].[Location] AS L ON R.[LocationParentId] = L.[Id] 
												  WHERE 1 = 1 '

		IF (@DeliveryType <> 4)
			SET @SqlStr = @SqlStr + N' AND R.[DeliveryType] = @DeliveryType '

		IF (@LocationChildId <> 0 AND @LocationId <> 0)
			SET @SqlStr = @SqlStr + N' AND R.[LocationId] = @LocationChildId'
		ELSE
			IF (@LocationId <> 0)
				SET @SqlStr = @SqlStr + N' AND R.[LocationParentId] = @LocationId'

		IF (@DeliveryPointId <> 0)
			SET @SqlStr = @SqlStr + N' AND R.[DeliveryPointId] = @DeliveryPointId '

		IF (@ProductId <> 0)
			SET @SqlStr = @SqlStr + N' AND R.[ProductId] = @ProductId '

		SET @SqlStr= @SqlStr + N' ORDER BY R.[ProductId] '
				
		EXEC SP_EXECUTESQL @SqlStr,
						   @ParamList,
						   @ProductId,
						   @LocationId,
						   @LocationChildId,
						   @DeliveryPointId,
						   @DeliveryType,
						   @Date

	END TRY
	BEGIN CATCH
		THROW;
	END CATCH
END
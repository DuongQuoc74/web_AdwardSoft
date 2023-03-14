﻿CREATE PROCEDURE [dbo].[usp_SupplierElastic_ReadById]
	@Id int
AS
	SELECT [Id],[Tel],[Phone],s.[Name],SC.[Name] as [NameContact]
	FROM [dbo].[Supplier] S
	INNER JOIN [dbo].[SupplierContact] SC ON S.[Id] = SC.[SupplierId] AND SC.[IsDefault] = 1
	WHERE S.[Id] = @Id

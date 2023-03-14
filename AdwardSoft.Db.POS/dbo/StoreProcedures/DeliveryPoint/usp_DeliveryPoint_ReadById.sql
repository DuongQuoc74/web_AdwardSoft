CREATE PROCEDURE [dbo].[usp_DeliveryPoint_ReadById]
	@Id INT
AS
	SELECT *
	FROM [dbo].[DeliveryPoint]
	WHERE [Id] = @Id


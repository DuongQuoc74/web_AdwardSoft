CREATE PROCEDURE [dbo].[usp_CustomerOrder_ReadById]
	@Id CHAR(32)
AS
	SELECT *
	FROM [dbo].[CustomerOrder]
	WHERE [Id] = @Id


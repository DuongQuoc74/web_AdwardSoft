CREATE PROCEDURE [dbo].[usp_CustomerWallet_ReadById]
	@Id CHAR(13)
AS
	SELECT * 
	FROM [dbo].[CustomerWallet]
	WHERE [Id] = @Id
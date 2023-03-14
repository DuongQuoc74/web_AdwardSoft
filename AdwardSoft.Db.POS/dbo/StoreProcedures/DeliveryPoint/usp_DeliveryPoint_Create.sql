CREATE PROCEDURE [dbo].[usp_DeliveryPoint_Create]
	@Id INT,
	@Code VARCHAR(6), 
	@Name NVARCHAR(120),
	@Rate NUMERIC(3),
	@LocationId INT,
	@Status TINYINT
AS
BEGIN
	--SET IDENTITY_INSERT [DeliveryPoint] ON
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
		BEGIN TRAN
				INSERT INTO [dbo].[DeliveryPoint]([Code],[Name],[Rate],[LocationId],[Status]) VALUES (@Code, @Name, @Rate, @LocationId, @Status)
			--ELSE
			--	INSERT INTO [dbo].[DeliveryPoint]([Code],[Name],[Rate],[Status]) VALUES (@Id, @Code, @Name, @Rate, @Status)
	SET @Id = SCOPE_IDENTITY()
	 
		COMMIT
		SELECT L.* 
		FROM [dbo].[DeliveryPoint] AS L
		WHERE L.[Id] = @Id
	END TRY
	BEGIN CATCH
		ROLLBACK;
		SELECT 0;
		THROW;
	END CATCH
END

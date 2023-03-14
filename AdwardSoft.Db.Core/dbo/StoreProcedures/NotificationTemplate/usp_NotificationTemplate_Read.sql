CREATE PROCEDURE [dbo].[usp_NotificationTemplate_Read]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM NotificationTemplate
END